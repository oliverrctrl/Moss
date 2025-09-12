namespace Moss.Core.Interfaces;

public interface ICoreScript
{
    /// <summary>
    /// Called when the script starts for the first time
    /// </summary>
    public void Start();
    
    /// <summary>
    /// Called every time the OS updates
    /// </summary>
    public void Update();
    
    /// <summary>
    /// Called when the OS wants the script to be stopped permanently, cleanup should be done here
    /// </summary>
    public void Stop();
}