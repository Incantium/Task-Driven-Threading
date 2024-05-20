namespace Obscurum.TDT.Tasks
{
    /// <summary>
    /// Interface that represents a single task with return type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface Task<out T>
    {
        /// <summary>
        /// Method called to execute the <see cref="Task{T}"/>.
        /// </summary>
        /// <returns>The return value of the <see cref="Task{T}"/>.</returns>
        public T Execute();
    }
    
    /// <summary>
    /// Class that represents a single task with no return type.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface Task
    { 
        /// <summary>
        /// Method called to execute the <see cref="Task"/>.
        /// </summary>
        void Execute();
    }
}