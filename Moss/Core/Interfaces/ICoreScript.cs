namespace Moss.Core.Interfaces
{
    public interface ICoreScript
    {
        /// <summary>
        /// Called when the script starts for the first time
        /// </summary>
        void Start();
        
        /// <summary>
        /// Called every time the OS updates
        /// </summary>
        void Update();
        
        /// <summary>
        /// Called when the OS wants the script to be stopped permanently
        /// </summary>
        void Stop();
    }
}