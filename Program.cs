using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class NotesManager
{
    private static string notesDirectory = "/home/okno/notes"; //your directory here for your notes (replace "okno" whit your name)
    private static string indexFile = Path.Combine(notesDirectory, "everynote.txt");

    static void Main(string[] args)
    {
        
        if (!Directory.Exists(notesDirectory))
        {
            Directory.CreateDirectory(notesDirectory);
        }

        
        if (!File.Exists(indexFile))
        {
            File.Create(indexFile).Close();
        }

        MainMenu();
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== NOTES ===");
            Console.WriteLine();
            Console.WriteLine("1 - Create new note");
            Console.WriteLine("2 - See older notes");
            Console.WriteLine("0 - Exit");
            Console.WriteLine();
            Console.Write("Your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateNewNote();
                    break;
                case "2":
                    ViewOldNotes();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("It seems like this is not a right!");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void CreateNewNote()
    {
        Console.Clear();
        Console.WriteLine("=== NEW NOTE ===");
        Console.WriteLine();
        Console.Write("Enter note name (whitout .txt): ");
        string noteName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(noteName))
        {
            Console.WriteLine("Your note has no name!");
            Console.ReadLine();
            return;
        }

        
        if (!noteName.EndsWith(".txt"))
        {
            noteName += ".txt";
        }

        
        File.AppendAllText(indexFile, noteName + Environment.NewLine);

        
        string noteFilePath = Path.Combine(notesDirectory, noteName);
        if (!File.Exists(noteFilePath))
        {
            File.Create(noteFilePath).Close();
        }

        Console.WriteLine();
        Console.WriteLine($"Note '{noteName}' was created!");
        Console.WriteLine("Now you can edit this note.");
        Console.WriteLine("Press enter . . .");
        Console.ReadLine();

        
        ViewAndEditNote(noteFilePath, noteName);
    }

    static void ViewOldNotes()
    {
        Console.Clear();
        Console.WriteLine("=== OLDER NOTES ===");
        Console.WriteLine();

        // Načtení seznamu poznámek
        if (!File.Exists(indexFile))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR");
            Console.ResetColor();
            Console.WriteLine("We did not find any notes!");
            Console.WriteLine("Press enter . . .");
            Console.ReadLine();
            return;
        }

        string[] notes = File.ReadAllLines(indexFile)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

        if (notes.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR");
            Console.ResetColor();
            Console.WriteLine("We did not find any notes!");
            Console.WriteLine("Press enter . . .");
            Console.ReadLine();
            return;
        }

        // Zobrazení seznamu poznámek s čísly
        for (int i = 0; i < notes.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {notes[i]}");
        }

        Console.WriteLine();
        Console.Write("Enter number of note, thet you want to see (for exit 0): ");
        
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice == 0)
            {
                return;
            }

            if (choice > 0 && choice <= notes.Length)
            {
                string selectedNote = notes[choice - 1];
                string noteFilePath = Path.Combine(notesDirectory, selectedNote);
                
                ViewAndEditNote(noteFilePath, selectedNote);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.ResetColor();
                Console.WriteLine("This number do not exist in here!");
                Console.WriteLine("Press enter . . .");
                Console.ReadLine();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR");
            Console.ResetColor();
            Console.WriteLine("Unknow enter!");
            Console.WriteLine("Press enter . . .");
            Console.ReadLine();
        }
    }

    static void ViewAndEditNote(string noteFilePath, string noteName)
    {
        Console.Clear();
        Console.WriteLine($"=== POZNÁMKA: {noteName} ===");
        Console.WriteLine();

        // Zobrazení obsahu poznámky
        if (File.Exists(noteFilePath))
        {
            string[] lines = File.ReadAllLines(noteFilePath);
            
            if (lines.Length == 0)
            {
                Console.WriteLine("(Nothing here . . .)");
            }
            else
            {
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("Soubor poznámky neexistuje!");
        }

        Console.WriteLine();
        Console.WriteLine("---");
        Console.WriteLine();
        Console.Write("Do you want to edit note? (Y/N): ");
        string editChoice = Console.ReadLine()?.ToLower();

        if (editChoice == "y" || editChoice == "yes")
        {
            EditNote(noteFilePath, noteName);
        }
    }

    static void EditNote(string noteFilePath, string noteName)
    {
        Console.Clear();
        Console.WriteLine($"=== EDITACE: {noteName} ===");
        Console.WriteLine();
        Console.WriteLine("Add new text (for exit press enter on empty line):");
        Console.WriteLine();

        List<string> newLines = new List<string>();
        
        while (true)
        {
            string line = Console.ReadLine();
            
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            
            newLines.Add(line);
        }

        if (newLines.Count > 0)
        {
            File.AppendAllLines(noteFilePath, newLines);
            Console.WriteLine();
            Console.WriteLine("Note was saved!");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Nothing new here . . .");
        }

        Console.WriteLine("Press enter . . .");
        Console.ReadLine();
    }
}

