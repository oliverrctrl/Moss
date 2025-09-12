using System;
using Moss.core.interfaces;
// using NLua;
using Moss.core.attributes;

namespace Moss.user.programs;

[Program("lua", "Lua Interpreter", "Lua to C# interpreter")]
public class LuaInterpreter : ISharpProgram
{
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