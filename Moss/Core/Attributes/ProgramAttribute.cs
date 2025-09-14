using System;

namespace Moss.Core.Attributes;

public class ProgramAttribute : Attribute
{
    public string Command { get; }
    public string Name { get; }
    public string Description { get; }

    public ProgramAttribute(string command, string name, string description)
    {
        Command = command;
        Name = name;
        Description = description;
    }
}