using Cryptography_Demo.Services;

Console.WriteLine("==== Secure Notes Vault ====");

// Step 1 — Secure password hashing
PasswordService passwordService = new PasswordService();
string password;
while (true)
{
    Console.Write("Set a password: ");
    password = Console.ReadLine() ?? string.Empty;

    if (!string.IsNullOrWhiteSpace(password))
        break;

    Console.WriteLine("Please enter a password. Password cannot be empty.\n");
}

// Generate hash and salt for the entered password
(string hash, string salt) = passwordService.HashPassword(password);
Console.WriteLine("\n[Password securely hashed and salted]\n");

// Step 2 — Input validation for the note
Console.Write("Enter your secret note: ");
string note = Console.ReadLine() ?? string.Empty;

if (!InputValidator.IsValidText(note))
{
    Console.WriteLine("Invalid note input. Unsafe characters detected.");
    return;
}

// Step 3 — Encrypt the note
EncryptionService encryptionService = new EncryptionService(password);
string encryptedNote = encryptionService.Encrypt(note);
Console.WriteLine($"\nEncrypted Note:\n{encryptedNote}");

// Step 4 — Verify password before decrypting
Console.Write("\nRe-enter password for verification: ");
string passwordAttempt = Console.ReadLine() ?? string.Empty;

bool isValid = passwordService.VerifyPassword(passwordAttempt, hash, salt);

if (isValid)
{
    Console.WriteLine("Password verified successfully.");
    // Only decrypt if password is correct
    string decryptedNote = encryptionService.Decrypt(encryptedNote);
    Console.WriteLine($"\nDecrypted Note:\n{decryptedNote}");
}
else
{
    Console.WriteLine("Invalid password. Access denied.");
}

Console.WriteLine("\n==== End of Demo ====");