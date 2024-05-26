namespace Obscurum.TDT.Runners
{
    /// <summary>
    /// Interface for classes that can run <see cref="Task"/> or <see cref="Task{T}"/> on a separate thread.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal interface BaseRunner
    {
        /// <summary>
        /// Method to start the <see cref="BaseRunner"/>.
        /// </summary>
        void Start();
    }
}