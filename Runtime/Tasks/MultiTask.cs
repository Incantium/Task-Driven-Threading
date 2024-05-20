namespace Obscurum.TDT.Tasks
{
    /// <summary>
    /// Interface that represents multiple likewise tasks with return type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface MultiTask<out T>
    {
        /// <summary>
        /// Method called to execute the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask{T}"/>.</param>
        /// <returns>The return value of the <see cref="MultiTask{T}"/>.</returns>
        T Execute(int i);
    }
    
    /// <summary>
    /// Interface that represents multiple likewise tasks with no return type.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface MultiTask
    {
        /// <summary>
        /// Method called to execute the <see cref="MultiTask"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask"/>.</param>
        void Execute(int i);
    }
}