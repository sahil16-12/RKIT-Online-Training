using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SecureNotes.Models;
using SecureNotes.Services;
using SecureNotes.Utilities;

/// <summary>
/// Entry point for SecureNotes CLI application.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main interactive loop.
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.WriteLine("SecureNotes — minimal secure note keeper (CLI)");
        Console.WriteLine("Type 'help' for commands.");

        string vaultPath = null;
        VaultService vaultService = null;
        CryptoService cryptoService = null;

        while (true)
        {
            Console.Write("> ");
            string? rawInput = Console.ReadLine();
            if (rawInput == null)
            {
                continue;
            }

            string[] tokens = rawInput.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
            {
                continue;
            }

            string command = tokens[0].ToLowerInvariant();
            string argument = tokens.Length > 1 ? tokens[1] : string.Empty;

            try
            {
                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;

                    case "init":
                        {
                            string[] parts = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 0)
                            {
                                Console.WriteLine("Usage: init <vault-path>");
                                break;
                            }

                            string path = parts[0];
                            Console.Write("Create passphrase: ");
                            string passphrase = ConsoleUtil.ReadPasswordMasked();
                            Console.WriteLine();
                            VaultService created = await VaultService.InitializeVaultAsync(path, passphrase);
                            Console.WriteLine("Vault initialized at: " + path);
                            break;
                        }

                    case "open":
                        {
                            string[] parts = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 0)
                            {
                                Console.WriteLine("Usage: open <vault-path>");
                                break;
                            }

                            string path = parts[0];
                            Console.Write("Enter passphrase: ");
                            string passphrase = ConsoleUtil.ReadPasswordMasked();
                            Console.WriteLine();
                            VaultService opened = await VaultService.OpenVaultAsync(path, passphrase);
                            vaultPath = path;
                            vaultService = opened;
                            // cryptoService is internal to VaultService and not leaked.
                            Console.WriteLine("Vault opened.");
                            break;
                        }

                    case "list":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            System.Collections.Generic.List<Note> notes = await vaultService!.ListNotesAsync();
                            foreach (Note note in notes)
                            {
                                Console.WriteLine($"{note.Id}  {note.Title}  Created: {note.CreatedAt:O}  Updated: {note.UpdatedAt:O}");
                            }

                            break;
                        }

                    case "create":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            Console.Write("Title: ");
                            string? title = Console.ReadLine() ?? string.Empty;
                            Console.WriteLine("Body (end with a single '.' on its own line):");
                            StringBuilder bodyBuilder = new StringBuilder();
                            while (true)
                            {
                                string? line = Console.ReadLine();
                                if (line == ".") break;
                                if (line == null) break;
                                bodyBuilder.AppendLine(line);
                            }

                            Note createdNote = await vaultService!.CreateNoteAsync(title, bodyBuilder.ToString());
                            Console.WriteLine("Created note: " + createdNote.Id);
                            break;
                        }

                    case "read":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            if (string.IsNullOrWhiteSpace(argument))
                            {
                                Console.WriteLine("Usage: read <note-id>");
                                break;
                            }

                            try
                            {
                                Note note = await vaultService!.ReadNoteAsync(argument);
                                Console.WriteLine("Title: " + note.Title);
                                Console.WriteLine("Created: " + note.CreatedAt.ToString("O"));
                                Console.WriteLine("Updated: " + note.UpdatedAt.ToString("O"));
                                Console.WriteLine("Body:");
                                Console.WriteLine(note.Body);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error reading note: " + e.Message);
                            }

                            break;
                        }

                    case "update":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            if (string.IsNullOrWhiteSpace(argument))
                            {
                                Console.WriteLine("Usage: update <note-id>");
                                break;
                            }

                            try
                            {
                                Note note = await vaultService!.ReadNoteAsync(argument);
                                Console.WriteLine("Current Title: " + note.Title);
                                Console.Write("New Title (leave blank to keep): ");
                                string? newTitle = Console.ReadLine();
                                Console.WriteLine("Enter new body (end with a single '.' on its own line). Leave blank (press '.' immediately) to keep:");
                                StringBuilder bodyBuilder = new StringBuilder();
                                while (true)
                                {
                                    string? line = Console.ReadLine();
                                    if (line == ".") break;
                                    if (line == null) break;
                                    bodyBuilder.AppendLine(line);
                                }

                                string? finalTitle = string.IsNullOrWhiteSpace(newTitle) ? note.Title : newTitle;
                                string? finalBody = bodyBuilder.Length == 0 ? note.Body : bodyBuilder.ToString();
                                Note updated = await vaultService!.UpdateNoteAsync(argument, finalTitle, finalBody);
                                Console.WriteLine("Updated note: " + updated.Id);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error updating note: " + e.Message);
                            }

                            break;
                        }

                    case "delete":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            if (string.IsNullOrWhiteSpace(argument))
                            {
                                Console.WriteLine("Usage: delete <note-id>");
                                break;
                            }

                            bool removed = await vaultService!.DeleteNoteAsync(argument);
                            Console.WriteLine(removed ? "Deleted." : "Not found.");
                            break;
                        }

                    case "export-xml":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            string[] parts = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 0)
                            {
                                Console.WriteLine("Usage: export-xml <path-to-output-xml>");
                                break;
                            }

                            string xmlPath = parts[0];
                            await vaultService!.ExportMetadataToXmlAsync(xmlPath);
                            Console.WriteLine("Exported metadata to XML: " + xmlPath);
                            break;
                        }

                    case "import-xml":
                        {
                            if (EnsureVaultOpen(vaultService) == false) break;
                            string[] parts = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 0)
                            {
                                Console.WriteLine("Usage: import-xml <path-to-xml>");
                                break;
                            }

                            string xmlPath = parts[0];
                            await vaultService!.ImportMetadataFromXmlAsync(xmlPath);
                            Console.WriteLine("Imported metadata from XML: " + xmlPath);
                            break;
                        }

                    case "close":
                        {
                            if (vaultService != null)
                            {
                                vaultService.Dispose();
                                vaultService = null;
                                vaultPath = null;
                                Console.WriteLine("Vault closed.");
                            }
                            else
                            {
                                Console.WriteLine("No vault is open.");
                            }

                            break;
                        }

                    case "exit":
                        {
                            if (vaultService != null)
                            {
                                vaultService.Dispose();
                                vaultService = null;
                            }

                            Console.WriteLine("Goodbye.");
                            return;
                        }

                    default:
                        {
                            Console.WriteLine("Unknown command. Type 'help' for commands.");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled error: " + ex.Message);
            }
        }
    }

    /// <summary>
    /// Show help text.
    /// </summary>
    private static void ShowHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  init <vault-path>         Initialize a new vault and set passphrase");
        Console.WriteLine("  open <vault-path>         Open an existing vault (prompts for passphrase)");
        Console.WriteLine("  close                     Close currently open vault");
        Console.WriteLine("  list                      List all notes (metadata only)");
        Console.WriteLine("  create                    Create a new note (multi-line body, end with '.' alone)");
        Console.WriteLine("  read <note-id>            Read and decrypt a note");
        Console.WriteLine("  update <note-id>          Update title/body of a note");
        Console.WriteLine("  delete <note-id>          Delete a note");
        Console.WriteLine("  export-xml <file>         Export metadata (no bodies) to XML");
        Console.WriteLine("  import-xml <file>         Import metadata from XML (no bodies)");
        Console.WriteLine("  help                      Show this help");
        Console.WriteLine("  exit                      Exit");
    }

    /// <summary>
    /// Ensure a vault is open and report if not.
    /// </summary>
    private static bool EnsureVaultOpen(VaultService? vaultService)
    {
        if (vaultService == null)
        {
            Console.WriteLine("No vault is open. Use 'open <vault-path>' or 'init <vault-path>'.");
            return false;
        }

        return true;
    }
}
