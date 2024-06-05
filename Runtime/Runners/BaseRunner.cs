namespace Incantium.TDT.Runners
{
    /// <summary>
    /// Interface for classes that can run <see cref="Task{T}"/> or <see cref="Task"/> on a separate thread.
    /// </summary>
    internal interface BaseRunner
    {
        /// <summary>
        /// Method to start the <see cref="BaseRunner"/>.
        /// </summary>
        /// <since>0.1.0</since>
        void Start();
    }
}