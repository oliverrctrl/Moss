using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Cosmos.System.Graphics;
using Moss.Core.Attributes;
using Moss.Core.Graphics;
using Moss.Core.Interfaces;
using Moss.Core.Shell;
using Sys = Cosmos.System;

namespace Moss.User.Programs;

[Program("programs", "Program List", "Displays a list of all known C# class-based and Lua programs")]
public class Programs : ISharpProgram
{
    public Window Window { get; set; }
    public WindowCanvas Canvas { get; set; }

    public void Run(string[] args)
    {
        var programs = ProgramRegistry.GetCommandNames().ToList();
        for (var i = 0; i < programs.Count; i++)
        {
            Canvas.Write(i.ToString());
            Canvas.Write(" " + programs[i]);
        }
    }

    public void Help()
    {
        
    }
}