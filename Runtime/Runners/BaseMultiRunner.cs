namespace Obscurum.TDT.Runners
{
    /// <summary>
    /// Interface for classes that can run <see cref="MultiTask"/> or <see cref="MultiTask{T}"/> on a
    /// separate thread.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal interface BaseMultiRunner
    {
        /// <summary>
        /// Method to start the <see cref="BaseMultiRunner"/>.
        /// </summary>
        /// <param name="amount">The amount of tasks.</param>
        void Start(int amount);
    }
}