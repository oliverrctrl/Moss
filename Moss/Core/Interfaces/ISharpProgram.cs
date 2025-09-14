using Moss.Core.Graphics;

namespace Moss.Core.Interfaces;

public interface ISharpProgram
{
    public Window Window { get; set; }
    public WindowCanvas Canvas { get; set; }
    
    /// <summary>
    /// Called when a program is initiated via the shell
    /// </summary>
    /// <param name="args">Arguments after the command</param>
    public void Run(string[] args);

    /// <summary>
    /// Called to display program information on the shell.
    /// </summary>
    public void Help();
}