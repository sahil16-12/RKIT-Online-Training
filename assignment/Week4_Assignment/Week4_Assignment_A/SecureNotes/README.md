# SecureNotes (CLI)

Minimal secure note keeper focusing on basic crypto hygiene.

## Overview

SecureNotes is a console application that stores notes as JSON files in a vault folder. Note bodies are encrypted using AES-GCM with a key derived from a user passphrase via PBKDF2 (Rfc2898DeriveBytes).

Each note has:

- `Id` (GUID)
- `Title` (plaintext metadata)
- `Body` (encrypted on-disk; decrypted in memory when requested)
- `CreatedAt`, `UpdatedAt` (DateTimeOffset)

Supported commands:

- `init <vault-path>` — create vault and set passphrase
- `open <vault-path>` — open vault (enter passphrase)
- `create` — create a new note
- `read <note-id>` — decrypt and display a note
- `update <note-id>` — update title/body
- `delete <note-id>` — delete note
- `list` — list all notes metadata
- `export-xml <file>` — export metadata (no bodies) to XML
- `import-xml <file>` — import metadata from XML (creates stub notes, no bodies)
- `close` — close vault (zeroes key)
- `exit` — exit CLI

---

## Build & Run

1. Create project: `dotnet new console -n SecureNotes`
2. Add files from this repo into the project (preserve namespaces).
3. Target .NET 7 or 8.
4. Build: `dotnet build`
5. Run: `dotnet run --project SecureNotes`

---

## Design & Implementation Notes

- **Key derivation:** PBKDF2 (Rfc2898) with SHA-256 and 100,000 iterations. A 32-byte random salt is generated and saved inside `vault.meta.json`. The derived key length is 32 bytes (AES-256).
- **Encryption:** AES-GCM (AesGcm) with 12-byte nonce and 16-byte tag. The encrypted payload on disk is stored as base64 strings: `CipherTextBase64`, `NonceBase64`, `TagBase64`.
- **Serialization:** `System.Text.Json` is used with a custom `DateTimeOffsetConverter` ensuring ISO-8601 round-trip format.
- **Metadata exposure:** Titles, created/updated timestamps, and IDs are stored in plaintext in JSON files to enable listing. Bodies are encrypted.
- **XML export/import (stretch):** Exports only metadata (Id, Title, CreatedAt, UpdatedAt). Import can create stub notes (empty bodies) or update metadata for existing notes.

---

### Protections implemented

1. **Encryption of sensitive data:** Note bodies are encrypted with AES-GCM using a derived key. This protects confidentiality and integrity of note bodies.
2. **Authenticated encryption (AES-GCM):** Detects tampering at decryption time.
3. **PBKDF2 key derivation:** Uses a random 32-byte salt stored in vault metadata and a high iteration count (100,000) to slow brute-force attacks.
4. **No secrets in logs:** The CLI avoids printing passphrases or keys.
5. **Masked passphrase input:** Passphrase entry is masked (asterisks).
6. **Zeroing sensitive buffers:** Where possible, byte arrays holding key material and plaintext are zeroed after use using `CryptographicOperations.ZeroMemory`.
7. **Minimal attack surface:** The app is a local CLI with no network components.

---
