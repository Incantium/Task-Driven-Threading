using Obscurum.TDT.Runners;

namespace Obscurum.TDT
{
    /// <summary>
    /// Interface that represents a single task with return type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface Task<T>
    {
        /// <summary>
        /// Method called to execute the <see cref="Task{T}"/>.
        /// </summary>
        /// <returns>The return value of the <see cref="Task{T}"/>.</returns>
        T Execute();
        
        /// <summary>
        /// Method to schedule a <see cref="Task{T}"/> for execution.
        /// </summary>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task{T}"/> to take in milliseconds.
        /// </param>
        /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        public Tracker<T> Schedule(
            int timeout = 0)
        {
            var tracker = new Tracker<T>();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner<T>(this, tracker, timeout) : 
                new Runner<T>(this, tracker);
            
            runner.Start(); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="Task{T}"/> for execution.
        /// </summary>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task{T}"/> depends upon.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task{T}"/> to take in milliseconds.
        /// </param>
        /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        public Tracker<T> Schedule(
            Tracker dependency,
            int timeout = 0)
        {
            var tracker = new Tracker<T>();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner<T>(this, tracker, timeout) : 
                new Runner<T>(this, tracker);
            
            dependency.dependency += () => runner.Start();

            return tracker;
        }
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
        
        /// <summary>
        /// Method to schedule a <see cref="Task"/> for execution.
        /// </summary>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task"/> to take in milliseconds.
        /// </param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        public Tracker Schedule(
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner(this, tracker, timeout) : 
                new Runner(this, tracker);
            
            runner.Start(); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="Task"/> for execution.
        /// </summary>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task"/> depends upon.</param>
        /// <param name="timeout">The maximum allotted time for the <see cref="Task"/> to take in milliseconds.
        /// </param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        public Tracker Schedule(
            Tracker dependency,
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseRunner runner = timeout > 0 ? 
                new TimeoutRunner(this, tracker, timeout) : 
                new Runner(this, tracker);
            
            dependency.dependency += () => runner.Start();

            return tracker;
        }
    }
}