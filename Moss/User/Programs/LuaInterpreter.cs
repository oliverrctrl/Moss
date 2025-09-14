using System;
// using NLua;
using Moss.Core.Attributes;
using Moss.Core.Graphics;
using Moss.Core.Interfaces;

namespace Moss.User.Programs;

[Program("lua", "Lua Interpreter", "Lua to C# interpreter")]
public class LuaInterpreter : ISharpProgram
{
    public Window Window { get; set; }
    public WindowCanvas Canvas { get; set; }

    public void Run(string[] args)
    {
        /* if (args.Length > 1)
        {
            var state = new Lua();
                
            var version = state.DoString("return _VERSION");
            var results = state.DoString(args[0]);

            if (args[1] != "--silent")
            {
                ShellManager.Instance.TargetInstance.WriteLine($"Lua-CSharp Interpreter (running {version})");
            }

            ShellManager.Instance.TargetInstance.WriteLine(results);
        }
        else
        {
            throw new ArgumentException("Missing argument");
        } */
    }

    public void Help()
    {
        
    }
}