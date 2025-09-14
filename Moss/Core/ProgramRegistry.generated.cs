using System;
using System.Collections.Generic;
using Moss.Core.Interfaces;
using Moss.User.Programs;

namespace Core
{
    public static class ProgramRegistry
    {
        private static Dictionary<string, Func<ISharpProgram>> commands = new Dictionary<string, Func<ISharpProgram>>();

        static ProgramRegistry()
        {
            commands["programs"] = () => new Programs();
            commands["lua"] = () => new LuaInterpreter();
            commands["fs"] = () => new Filesystem();
        }

        public static IEnumerable<string> GetCommandNames()
        {
            return commands.Keys;
        }

        public static ISharpProgram CreateProgram(string name)
        {
            name = name.ToLower();
            return commands.ContainsKey(name) ? commands[name]() : null;
        }
    }
}
