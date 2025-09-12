using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Moss.core.attributes;
using Moss.core.interfaces;
using Moss.user.shell;
using Sys = Cosmos.System;

namespace Moss.user.programs;

[Program("programs", "Program List", "Displays a list of all known C# class-based and Lua programs")]
public class Programs : ISharpProgram
{
    public void Run(string[] args)
    {
        var programs = ProgramRegistry.GetCommandNames().ToList();
        for (var i = 0; i < programs.Count; i++)
        {
            ShellManager.Instance.TargetInstanceForegroundColor = ConsoleColor.Gray;
            ShellManager.Instance.TargetInstance.Write(i);
            ShellManager.Instance.TargetInstance.ForegroundColor = Kernel.ShellManager.TargetInstance.ForegroundColor;
            ShellManager.Instance.TargetInstance.WriteLine(" " + programs[i]);
        }
    }

    public void Help()
    {
        
    }
}