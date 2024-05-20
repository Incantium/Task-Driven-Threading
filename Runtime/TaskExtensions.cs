using Obscurum.TDT.Runners;
using Obscurum.TDT.Tasks;

namespace Obscurum.TDT
{
    /// <summary>
    /// Extension class to schedule a <see cref="Task{T}"/> or <see cref="Task"/> for execution.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public static class TaskExtensions
    {
        /// <summary>
        /// Method to schedule a <see cref="Task{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="Task{T}"/> to execute.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task{T}"/> to take in milliseconds.
        /// </param>
        /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        public static Tracker<T> Schedule<T>(
            this Task<T> task,
            int timeout = 0)
        {
            var tracker = new Tracker<T>();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner<T>(task, tracker, timeout) : 
                new Runner<T>(task, tracker);
            
            runner.Start(); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="Task{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="Task{T}"/> to execute.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task{T}"/> depends upon.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task{T}"/> to take in milliseconds.
        /// </param>
        /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        public static Tracker<T> Schedule<T>(
            this Task<T> task,
            Tracker dependency,
            int timeout = 0)
        {
            var tracker = new Tracker<T>();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner<T>(task, tracker, timeout) : 
                new Runner<T>(task, tracker);
            
            dependency.dependency += () => runner.Start();

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="Task"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to execute.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task"/> to take in milliseconds.
        /// </param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        public static Tracker Schedule(
            this Task task,
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner(task, tracker, timeout) : 
                new Runner(task, tracker);
            
            runner.Start(); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="Task"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to execute.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task"/> depends upon.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task"/> to take in milliseconds.
        /// </param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        public static Tracker Schedule(
            this Task task,
            Tracker dependency,
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner(task, tracker, timeout) : 
                new Runner(task, tracker);
            
            dependency.dependency += () => runner.Start();

            return tracker;
        }
    }
}