using System.Text.RegularExpressions;

var exePath = AppContext.BaseDirectory;
var solutionRoot = Path.GetFullPath(Path.Combine(exePath, "../../../../Moss"));
var programsDir = Path.Combine(solutionRoot, "User/Programs");
var outputFile = Path.Combine(solutionRoot, "Core/ProgramRegistry.generated.cs");

var files = Directory.GetFiles(programsDir, "*.cs");
var programs = new List<(string name, string className)>();

foreach (var file in files)
{
    string content = File.ReadAllText(file);
    var match = Regex.Match(content, @"\[Program\(""([^"",]+)"""); // first argument only
    var classMatch = Regex.Match(content, @"public\s+class\s+(\w+)");
    if (match.Success && classMatch.Success)
    {
        programs.Add((match.Groups[1].Value, classMatch.Groups[1].Value));
    }
}

using (var writer = new StreamWriter(outputFile))
{
    writer.WriteLine("using System;");
    writer.WriteLine("using System.Collections.Generic;");
    writer.WriteLine("using Moss.Core.Interfaces;");
    writer.WriteLine("using Moss.User.Programs;");
    writer.WriteLine();
    writer.WriteLine("namespace Core");
    writer.WriteLine("{");
    writer.WriteLine("    public static class ProgramRegistry");
    writer.WriteLine("    {");
    writer.WriteLine("        private static Dictionary<string, Func<ISharpProgram>> commands = new Dictionary<string, Func<ISharpProgram>>();");
    writer.WriteLine();
    writer.WriteLine("        static ProgramRegistry()");
    writer.WriteLine("        {");
    foreach (var (name, cls) in programs)
    {
        writer.WriteLine($"commands[\"{name.ToLower()}\"] = () => new {cls}();");
    }
    writer.WriteLine("        }");
    writer.WriteLine();
    writer.WriteLine("        public static IEnumerable<string> GetCommandNames()");
    writer.WriteLine("        {");
    writer.WriteLine("            return commands.Keys;");
    writer.WriteLine("        }");
    writer.WriteLine();
    writer.WriteLine("        public static ISharpProgram CreateProgram(string name)");
    writer.WriteLine("        {");
    writer.WriteLine("            name = name.ToLower();");
    writer.WriteLine("            return commands.ContainsKey(name) ? commands[name]() : null;");
    writer.WriteLine("        }");
    writer.WriteLine("    }");
    writer.WriteLine("}");
}

Console.WriteLine($"Generated registry for {programs.Count} programs.");
