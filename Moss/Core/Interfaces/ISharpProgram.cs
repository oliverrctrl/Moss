namespace Moss.core.interfaces;

public interface ISharpProgram
{
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