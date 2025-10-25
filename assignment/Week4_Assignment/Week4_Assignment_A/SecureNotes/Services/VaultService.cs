using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using SecureNotes.Models;
using SecureNotes.Json;
using System.Security.Cryptography;

/// <summary>
/// Manages vault lifecycle: initialize, open, create/read/update/delete notes, XML export/import.
/// Each note is stored as a JSON file named {guid}.json. Vault metadata is stored in vault.meta.json.
/// </summary>
namespace SecureNotes.Services
{
    public sealed class VaultService : IDisposable
    {
        /// <summary>File name for vault metadata.</summary>
        private const string VaultMetaFileName = "vault.meta.json";

        /// <summary>Directory path for the vault.</summary>
        private readonly string _vaultDirectory;

        /// <summary>Crypto service instance used for encryption/decryption.</summary>
        private readonly CryptoService _cryptoService;

        /// <summary>Json serializer options configured with DateTimeOffset converter.</summary>
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>Whether this instance has been disposed.</summary>
        private bool _disposed;

        /// <summary>
        /// Internal metadata persisted for vault: salt and iteration count.
        /// </summary>
        private sealed class VaultMeta
        {
            public string SaltBase64 { get; set; }
            public int Iterations { get; set; }

            public VaultMeta()
            {
                SaltBase64 = string.Empty;
                Iterations = 100_000;
            }
        }

        /// <summary>
        /// Initialize and create a new vault on disk and return an opened VaultService.
        /// </summary>
        public static async Task<VaultService> InitializeVaultAsync(string vaultDirectory, string passphrase)
        {
            if (string.IsNullOrWhiteSpace(vaultDirectory))
            {
                throw new ArgumentException("Invalid vault path", nameof(vaultDirectory));
            }

            DirectoryInfo dir = Directory.CreateDirectory(vaultDirectory);

            byte[] salt = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            int iterations = 100_000;

            VaultMeta meta = new VaultMeta();
            meta.SaltBase64 = Convert.ToBase64String(salt);
            meta.Iterations = iterations;

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeOffsetConverter());
            string metaJson = JsonSerializer.Serialize(meta, options);

            string metaPath = Path.Combine(dir.FullName, VaultMetaFileName);
            await File.WriteAllTextAsync(metaPath, metaJson, Encoding.UTF8);

            CryptoService cryptoService = new CryptoService(passphrase, salt, iterations);

            VaultService service = new VaultService(dir.FullName, cryptoService);
            return service;
        }

        /// <summary>
        /// Open an existing vault using passphrase (validates by trying to decrypt a note if present).
        /// </summary>
        public static async Task<VaultService> OpenVaultAsync(string vaultDirectory, string passphrase)
        {
            if (string.IsNullOrWhiteSpace(vaultDirectory))
            {
                throw new ArgumentException("Invalid vault path", nameof(vaultDirectory));
            }

            if (!Directory.Exists(vaultDirectory))
            {
                throw new DirectoryNotFoundException("Vault directory does not exist: " + vaultDirectory);
            }

            string metaPath = Path.Combine(vaultDirectory, VaultMetaFileName);
            if (!File.Exists(metaPath))
            {
                throw new FileNotFoundException("Vault metadata not found. Is this a vault?");
            }

            string metaJson = await File.ReadAllTextAsync(metaPath, Encoding.UTF8);
            JsonSerializerOptions metaOptions = new JsonSerializerOptions();
            metaOptions.Converters.Add(new DateTimeOffsetConverter());
            VaultMeta meta = JsonSerializer.Deserialize<VaultMeta>(metaJson, metaOptions) ?? new VaultMeta();

            byte[] salt = Convert.FromBase64String(meta.SaltBase64);
            int iterations = meta.Iterations;

            CryptoService cryptoService = new CryptoService(passphrase, salt, iterations);

            VaultService service = new VaultService(vaultDirectory, cryptoService);

            // Optional: Validate passphrase by decrypting first note if present.
            string[] noteFiles = Directory.GetFiles(vaultDirectory, "*.json", SearchOption.TopDirectoryOnly);
            foreach (string file in noteFiles)
            {
                if (Path.GetFileName(file) == VaultMetaFileName) continue;
                // Try to read and decrypt; if a decrypt fails with CryptographicException, passphrase is invalid.
                try
                {
                    using FileStream fs = File.OpenRead(file);
                    JsonDocument doc = await JsonDocument.ParseAsync(fs);
                    if (doc.RootElement.TryGetProperty("EncryptedBody", out JsonElement encElement))
                    {
                        string cipherBase64 = encElement.GetProperty("CipherTextBase64").GetString() ?? string.Empty;
                        string nonceBase64 = encElement.GetProperty("NonceBase64").GetString() ?? string.Empty;
                        string tagBase64 = encElement.GetProperty("TagBase64").GetString() ?? string.Empty;

                        byte[] cipher = Convert.FromBase64String(cipherBase64);
                        byte[] nonce = Convert.FromBase64String(nonceBase64);
                        byte[] tag = Convert.FromBase64String(tagBase64);

                        try
                        {
                            byte[] plaintext = service._cryptoService.Decrypt(cipher, nonce, tag);
                            // Zero plaintext
                            System.Security.Cryptography.CryptographicOperations.ZeroMemory(plaintext);
                        }
                        catch (CryptographicException)
                        {
                            service.Dispose();
                            throw new UnauthorizedAccessException("Invalid passphrase for vault.");
                        }
                    }
                }
                catch (JsonException)
                {
                    // If file corrupt, skip validation but keep service open — user may want to recover.
                }
            }

            return service;
        }

        /// <summary>
        /// Private constructor; callers should use InitializeVaultAsync or OpenVaultAsync.
        /// </summary>
        private VaultService(string vaultDirectory, CryptoService cryptoService)
        {
            _vaultDirectory = vaultDirectory;
            _cryptoService = cryptoService;
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new DateTimeOffsetConverter());
            _disposed = false;
        }

        /// <summary>
        /// Create a new note: encrypt body and persist a note JSON file.
        /// </summary>
        public async Task<Note> CreateNoteAsync(string title, string body)
        {
            Note note = new Note();
            note.Id = Guid.NewGuid();
            note.Title = title ?? string.Empty;
            note.Body = body ?? string.Empty;
            note.CreatedAt = DateTimeOffset.UtcNow;
            note.UpdatedAt = note.CreatedAt;

            EncryptedPayload payload = await EncryptBodyAsync(note.Body);

            // Build disk object
            var diskObject = new
            {
                Id = note.Id,
                Title = note.Title,
                EncryptedBody = payload,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };

            string json = JsonSerializer.Serialize(diskObject, _jsonOptions);
            string filePath = Path.Combine(_vaultDirectory, note.Id.ToString() + ".json");
            await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);

            // Zero plaintext in memory string — best-effort (strings are immutable)
            // If extremely sensitive, use secure APIs and avoid creating long-lived strings.
            return note;
        }

        /// <summary>
        /// Read and decrypt a note by id. Throws if not found or decryption fails.
        /// </summary>
        public async Task<Note> ReadNoteAsync(string id)
        {
            string filePath = Path.Combine(_vaultDirectory, id + ".json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Note not found.");
            }

            string json = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            Note note = new Note();
            note.Id = root.GetProperty("Id").GetGuid();
            note.Title = root.GetProperty("Title").GetString() ?? string.Empty;
            note.CreatedAt = root.GetProperty("CreatedAt").GetDateTimeOffset();
            note.UpdatedAt = root.GetProperty("UpdatedAt").GetDateTimeOffset();

            JsonElement encElement = root.GetProperty("EncryptedBody");
            string cipherBase64 = encElement.GetProperty("CipherTextBase64").GetString() ?? string.Empty;
            string nonceBase64 = encElement.GetProperty("NonceBase64").GetString() ?? string.Empty;
            string tagBase64 = encElement.GetProperty("TagBase64").GetString() ?? string.Empty;

            byte[] cipher = Convert.FromBase64String(cipherBase64);
            byte[] nonce = Convert.FromBase64String(nonceBase64);
            byte[] tag = Convert.FromBase64String(tagBase64);

            byte[] plaintextBytes = _cryptoService.Decrypt(cipher, nonce, tag);
            string body = Encoding.UTF8.GetString(plaintextBytes);

            // Zero plaintext bytes buffer
            System.Security.Cryptography.CryptographicOperations.ZeroMemory(plaintextBytes);

            note.Body = body;
            return note;
        }

        /// <summary>
        /// Update an existing note's title and body.
        /// </summary>
        public async Task<Note> UpdateNoteAsync(string id, string newTitle, string newBody)
        {
            string filePath = Path.Combine(_vaultDirectory, id + ".json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Note not found.");
            }

            Note note = await ReadNoteAsync(id);
            note.Title = newTitle ?? note.Title;
            note.Body = newBody ?? note.Body;
            note.UpdatedAt = DateTimeOffset.UtcNow;

            EncryptedPayload payload = await EncryptBodyAsync(note.Body);

            var diskObject = new
            {
                Id = note.Id,
                Title = note.Title,
                EncryptedBody = payload,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };

            string json = JsonSerializer.Serialize(diskObject, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);

            return note;
        }

        /// <summary>
        /// Delete a note by id. Returns true if deleted.
        /// </summary>
        public async Task<bool> DeleteNoteAsync(string id)
        {
            string filePath = Path.Combine(_vaultDirectory, id + ".json");
            if (!File.Exists(filePath))
            {
                return false;
            }

            await Task.Run(() => File.Delete(filePath));
            return true;
        }

        /// <summary>
        /// List all notes metadata (no bodies).
        /// </summary>
        public async Task<List<Note>> ListNotesAsync()
        {
            List<Note> result = new List<Note>();
            string[] files = Directory.GetFiles(_vaultDirectory, "*.json", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == VaultMetaFileName) continue;
                string json = await File.ReadAllTextAsync(file, Encoding.UTF8);
                try
                {
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;
                    Note note = new Note();
                    note.Id = root.GetProperty("Id").GetGuid();
                    note.Title = root.GetProperty("Title").GetString() ?? string.Empty;
                    note.CreatedAt = root.GetProperty("CreatedAt").GetDateTimeOffset();
                    note.UpdatedAt = root.GetProperty("UpdatedAt").GetDateTimeOffset();
                    note.Body = string.Empty;
                    result.Add(note);
                }
                catch (JsonException)
                {
                    // Skip corrupt file
                }
            }

            return result;
        }

        /// <summary>
        /// Export metadata (Id, Title, CreatedAt, UpdatedAt) to an XML file. Body is omitted.
        /// </summary>
        public async Task ExportMetadataToXmlAsync(string xmlPath)
        {
            List<Note> notes = await ListNotesAsync();
            XElement root = new XElement("Notes");
            foreach (Note note in notes)
            {
                XElement n = new XElement("Note",
                    new XElement("Id", note.Id.ToString()),
                    new XElement("Title", note.Title),
                    new XElement("CreatedAt", note.CreatedAt.ToString("O")),
                    new XElement("UpdatedAt", note.UpdatedAt.ToString("O"))
                );

                root.Add(n);
            }

            XDocument doc = new XDocument(root);
            await Task.Run(() => doc.Save(xmlPath));
        }

        /// <summary>
        /// Import metadata from XML. This only creates stub note files if they don't exist (no bodies).
        /// If a note exists, its metadata is updated but body is left unchanged.
        /// </summary>
        public async Task ImportMetadataFromXmlAsync(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("XML file not found.");
            }

            XDocument doc = XDocument.Load(xmlPath);
            XElement root = doc.Root ?? throw new InvalidOperationException("Invalid XML format.");

            foreach (XElement noteElem in root.Elements("Note"))
            {
                string idStr = noteElem.Element("Id")?.Value ?? string.Empty;
                string title = noteElem.Element("Title")?.Value ?? string.Empty;
                string createdAtStr = noteElem.Element("CreatedAt")?.Value ?? string.Empty;
                string updatedAtStr = noteElem.Element("UpdatedAt")?.Value ?? string.Empty;

                Guid id = Guid.Parse(idStr);
                DateTimeOffset createdAt = DateTimeOffset.Parse(createdAtStr);
                DateTimeOffset updatedAt = DateTimeOffset.Parse(updatedAtStr);

                string filePath = Path.Combine(_vaultDirectory, id.ToString() + ".json");
                if (!File.Exists(filePath))
                {
                    // create stub note with empty body
                    Note note = new Note();
                    note.Id = id;
                    note.Title = title;
                    note.Body = string.Empty;
                    note.CreatedAt = createdAt;
                    note.UpdatedAt = updatedAt;

                    EncryptedPayload payload = await EncryptBodyAsync(note.Body);

                    var diskObject = new
                    {
                        Id = note.Id,
                        Title = note.Title,
                        EncryptedBody = payload,
                        CreatedAt = note.CreatedAt,
                        UpdatedAt = note.UpdatedAt
                    };

                    string json = JsonSerializer.Serialize(diskObject, _jsonOptions);
                    await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
                }
                else
                {
                    // Update title and timestamps; preserve body
                    string existingJson = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                    using JsonDocument docExisting = JsonDocument.Parse(existingJson);
                    JsonElement rootExisting = docExisting.RootElement;
                    JsonElement enc = rootExisting.GetProperty("EncryptedBody");

                    var diskObject = new
                    {
                        Id = id,
                        Title = title,
                        EncryptedBody = new EncryptedPayload
                        {
                            CipherTextBase64 = enc.GetProperty("CipherTextBase64").GetString() ?? string.Empty,
                            NonceBase64 = enc.GetProperty("NonceBase64").GetString() ?? string.Empty,
                            TagBase64 = enc.GetProperty("TagBase64").GetString() ?? string.Empty
                        },
                        CreatedAt = createdAt,
                        UpdatedAt = updatedAt
                    };

                    string json = JsonSerializer.Serialize(diskObject, _jsonOptions);
                    await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
                }
            }
        }

        /// <summary>
        /// Helper to encrypt a body string into EncryptedPayload.
        /// </summary>
        private async Task<EncryptedPayload> EncryptBodyAsync(string body)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(body ?? string.Empty);
            try
            {
                (byte[] cipher, byte[] nonce, byte[] tag) = _cryptoService.Encrypt(plainBytes);
                EncryptedPayload payload = new EncryptedPayload();
                payload.CipherTextBase64 = Convert.ToBase64String(cipher);
                payload.NonceBase64 = Convert.ToBase64String(nonce);
                payload.TagBase64 = Convert.ToBase64String(tag);

                // Zero cipher/nonce/tag arrays if desired (they are copied into base64 strings)
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(cipher);
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(nonce);
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(tag);

                return payload;
            }
            finally
            {
                // Zero plaintext buffer
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(plainBytes);
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Dispose resources and zero key material.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cryptoService.Dispose();
        }
    }
}
