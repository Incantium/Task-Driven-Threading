namespace Incantium.TDT.Runners
{
    /// <summary>
    /// Interface for classes that can run <see cref="MultiTask{T}"/> or <see cref="MultiTask"/> on a
    /// separate thread.
    /// </summary>
    internal interface BaseMultiRunner
    {
        /// <summary>
        /// Method to start the <see cref="BaseMultiRunner"/>.
        /// </summary>
        /// <param name="amount">The amount of tasks.</param>
        /// <since>0.1.0</since>
        void Start(int amount);
    }
}