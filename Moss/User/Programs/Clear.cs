using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Moss.core.attributes;
using Moss.core.interfaces;
using Moss.user.shell;
using Sys = Cosmos.System;

namespace Moss.user.programs;

[Program("clear", "Clear", "Clears the shell & optionally sets the background color")]
public class Clear : ISharpProgram
{
    public void Run(string[] args)
    {
        ConsoleColor? bgColor = null;

        if (args.Length > 0)
            bgColor = ParseConsoleColor(args[0]);

        ShellManager.Instance.TargetInstance.BackgroundColor = bgColor ?? Kernel.ShellManager.TargetInstance.BackgroundColor;
        ShellManager.Instance.TargetInstance.ForegroundColor = Kernel.ShellManager.TargetInstance.ForegroundColor;
        ShellManager.Instance.TargetInstance.Clear();
    }

    public void Help()
    {
        ShellManager.Instance.TargetInstance.WriteLine("clear (optional: color)\n" +
                          "\tPossible colors: black, darkblue, darkgreen, darkcyan, darkred, darkmagenta, darkyellow, gray, darkgray, blue, green, cyan, red, magenta, yellow, white");
    }

    private static ConsoleColor? ParseConsoleColor(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return null;

        return str.ToLower() switch
        {
            "black" => ConsoleColor.Black,
            "darkblue" => ConsoleColor.DarkBlue,
            "darkgreen" => ConsoleColor.DarkGreen,
            "darkcyan" => ConsoleColor.DarkCyan,
            "darkred" => ConsoleColor.DarkRed,
            "darkmagenta" => ConsoleColor.DarkMagenta,
            "darkyellow" => ConsoleColor.DarkYellow,
            "gray" => ConsoleColor.Gray,
            "darkgray" => ConsoleColor.DarkGray,
            "blue" => ConsoleColor.Blue,
            "green" => ConsoleColor.Green,
            "cyan" => ConsoleColor.Cyan,
            "red" => ConsoleColor.Red,
            "magenta" => ConsoleColor.Magenta,
            "yellow" => ConsoleColor.Yellow,
            "white" => ConsoleColor.White,
            _ => null
        };
    }
}