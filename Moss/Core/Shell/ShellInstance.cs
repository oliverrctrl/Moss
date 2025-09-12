using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Moss.User.Shell;

/// <summary>
/// This is a single **individual** shell instance, it is automatically created via ShellContainer.
/// </summary>
public class ShellInstance
{
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

    private List<string> _programs;
    public ShellInstance()
    {
        _programs = ProgramRegistry.GetCommandNames().ToList();
        ShellManager.Instance.TargetInstance.Clear();
        ShellManager.Instance.TargetInstance.ForegroundColor = ConsoleColor.Green;
        ShellManager.Instance.TargetInstance.WriteLine($"Welcome to Moss, {_programs.Count} programs are available.\nType 'programs' to list them");
        ShellManager.Instance.TargetInstance.ForegroundColor = ForegroundColor;
    }

    public void Update()
    {
        ShellManager.Instance.TargetInstance.ForegroundColor = ConsoleColor.Yellow;
        ShellManager.Instance.TargetInstance.Write(">");
        ShellManager.Instance.TargetInstance.ForegroundColor = ForegroundColor;
        
        var input = ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return;

        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLower();
        var args = parts.Skip(1).ToArray();

        var isValid = _programs.Any(p => p == command);

        if (isValid)
        {
            try
            {
                var prog = ProgramRegistry.CreateProgram(command);
                
                if (args.Length != 0)
                {
                    // Probably a bad idea to have arguments like -h count as asking for help
                    // If this is problematic then best to remove them
                    string[] helpVariants = { "help", "--help", "-help", "-h", "--h" };
                    if (helpVariants.Contains(args[0]))
                    {
                        prog?.Help();
                    }
                    else
                    {
                        prog?.Run(args);
                    }
                }
                else
                {
                    prog?.Run(args);
                }
            }
            catch (Exception ex)
            {
                ShellManager.Instance.TargetInstance.ForegroundColor = ConsoleColor.Red;
                ShellManager.Instance.TargetInstance.WriteLine(
                    $"{ex.Message}");
                ShellManager.Instance.TargetInstance.ForegroundColor = ForegroundColor;
            }
        }
        else
        {
            ShellManager.Instance.TargetInstance.WriteLine("Invalid program. Type 'programs' for a list of valid programs");
        }
    }

    public void WriteLine(string text)
    {
        /*
        Console.WriteLine(text);
        */
    }

    public void Write(string text)
    {
        /*
        Console.Write(text);
        */
    }
    
    public void Clear()
    {
        /*
        Console.ForegroundColor = ForegroundColor;
        Console = BackgroundColor;
        Console.Clear();
        */
    }

    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey();
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }
}