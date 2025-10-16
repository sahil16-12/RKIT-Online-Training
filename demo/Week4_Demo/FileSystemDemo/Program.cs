using System;
using System.IO;

namespace FileSystemDemo;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string folderPath = "DemoFolder";
        string filePath = Path.Combine(folderPath, "sample.txt");

        // Create directory if it doesn't exist
        Directory.CreateDirectory(folderPath);

        CreateAndWriteFile(filePath);
        ReadFile(filePath);
        AppendToFile(filePath);
        ShowFileInfo(filePath);
        CopyAndMoveFile(filePath, folderPath);
        DirectoryOperations(folderPath);
    }

    // 1️ Create and write text into a file
    static void CreateAndWriteFile(string path)
    {
        string[] lines =
        [
            "Hello from C#",
            "This is a file system demo.",
            "We'll perform basic file operations here."
        ];

        File.WriteAllLines(path, lines);
        Console.WriteLine($" File created at: {Path.GetFullPath(path)}");
    }

    // 2️ Read file content
    static void ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine(" File not found for reading!");
            return;
        }

        Console.WriteLine("\nReading file content:");
        foreach (string line in File.ReadAllLines(path))
        {
            Console.WriteLine(line);
        }
    }

    // 3️⃣ Append more content to an existing file
    static void AppendToFile(string path)
    {
        File.AppendAllText(path, "\nAppended line at: " + DateTime.Now);
        Console.WriteLine("\n Appended new content to file.");
    }

    // 4️ Show file information (metadata)
    static void ShowFileInfo(string path)
    {
        FileInfo info = new(path);
        Console.WriteLine("\n File Info:");
        Console.WriteLine($"Name: {info.Name}");
        Console.WriteLine($"Extension: {info.Extension}");
        Console.WriteLine($"Size: {info.Length} bytes");
        Console.WriteLine($"Created: {info.CreationTime}");
        Console.WriteLine($"Last Modified: {info.LastWriteTime}");
        
        //// Windows HOME edition doesnt info.Encrypt().
    }

    // 5️ Copy, move, and delete operations
    static void CopyAndMoveFile(string path, string folderPath)
    {
        string copyPath = Path.Combine(folderPath, "sample_copy.txt");
        string movedPath = Path.Combine(folderPath, "sample_moved.txt");

        // Copy file (overwrite if already exists)
        File.Copy(path, copyPath, overwrite: true);
        Console.WriteLine("\n File copied successfully.");

        // Move (rename) file
        if (File.Exists(movedPath)) File.Delete(movedPath); // avoid name clash
        File.Move(copyPath, movedPath);
        Console.WriteLine(" File moved successfully.");

        // Delete the moved file
        File.Delete(movedPath);
        Console.WriteLine(" Moved file deleted.");
    }

    // 6️ Directory creation, listing, and deletion
    static void DirectoryOperations(string folderPath)
    {
        string subFolder = Path.Combine(folderPath, "SubFolder");
        Directory.CreateDirectory(subFolder);

        Console.WriteLine("\n Directory Operations:");
        Console.WriteLine("Created subfolder: " + subFolder);

        // List all files and folders
        Console.WriteLine("\nContents of main folder:");
        foreach (String file in Directory.GetFiles(folderPath))
            Console.WriteLine("File: " + file);

        foreach (String dir in Directory.GetDirectories(folderPath))
            Console.WriteLine("Folder: " + dir);

        // Cleanup demo folders (optional)
        Directory.Delete(folderPath, recursive: true);
        Console.WriteLine("\n Deleted demo folder and its contents.");
    }
}
