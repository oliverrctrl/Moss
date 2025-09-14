using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Core;
using Moss.Core.Graphics;
using Moss.Core.Graphics.Elements;
using Moss.Core.Interfaces;

namespace Moss.Core.Shell;

/// <summary>
/// This is a single **individual** shell instance, it is automatically created via ShellContainer.
/// </summary>
public class Shell : ICoreScript
{
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

    private List<string> _programs;

    private static Window _window;
    private WindowCanvas _canvas;
    public Shell(Window window)
    {
        _window  = window;
        _canvas = window.WindowCanvas;
    }

    private InputState _inputState = new();
    private Textbox _inputTextbox = new()
    {
        text = "",
        color = default,
        posX = 5,
        posY = _window.Height - 15,
        width = _window.Width - 10,
        placeholder = "Enter command"
    };
    
    public void Start()
    {
        _programs = ProgramRegistry.GetCommandNames().ToList();
        _canvas.Write($"Welcome to Moss, {_programs.Count} programs are available.\nType 'programs' to list them", Color.DarkSeaGreen);
    }

    public void Update()
    {
        var input = _canvas.ReadLine(_inputState, _inputTextbox);

        if (string.IsNullOrWhiteSpace(input))
            return;

        _inputState.Buffer = "";
        _inputState.IsComplete = false;

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
                    string[] helpVariants = { "help", "--help", "-help", "-h", "--h" };
                    if (helpVariants.Contains(args[0]))
                        prog?.Help();
                    else
                        prog?.Run(args);
                }
                else
                {
                    prog?.Run(args);
                }
            }
            catch (Exception ex)
            {
                _canvas.Write($"{ex.Message}", Color.Red);
            }
        }
        else
        {
            _canvas.Write("Invalid program. Type 'programs' for a list of valid programs");
        }
    }

    public void Stop()
    {
        
    }
}