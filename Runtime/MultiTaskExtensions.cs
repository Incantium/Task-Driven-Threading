using System.Collections;
using Obscurum.TDT.Runners;
using Obscurum.TDT.Tasks;

namespace Obscurum.TDT
{
    /// <summary>
    /// Extension class to schedule a <see cref="MultiTask{T}"/> or <see cref="MultiTask"/> for execution.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public static class MultiTaskExtensions
    {
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask{T}"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public static Tracker<T[]> Schedule<T>(
            this MultiTask<T> task,
            ICollection amount,
            int batch = 1, 
            int timeout = 0) => Schedule(task, amount.Count, batch, timeout);
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask{T}"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public static Tracker<T[]> Schedule<T>(
            this MultiTask<T> task, 
            int amount, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(task, tracker, batch, timeout) : 
                new MultiRunner<T>(task, tracker, batch);
            
            runner.Start(amount); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask{T}"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public static Tracker<T[]> Schedule<T>(
            this MultiTask<T> task,
            ICollection amount,
            Tracker dependency,
            int batch = 1, 
            int timeout = 0) => Schedule(task, amount.Count, dependency, batch, timeout);
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask{T}"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public static Tracker<T[]> Schedule<T>(
            this MultiTask<T> task, 
            int amount, 
            Tracker dependency, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(task, tracker, batch, timeout) : 
                new MultiRunner<T>(task, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount);

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public static Tracker Schedule(
            this MultiTask task,
            ICollection amount,
            int batch = 1, 
            int timeout = 0) => Schedule(task, amount.Count, batch, timeout);
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public static Tracker Schedule(
            this MultiTask task, 
            int amount, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(task, tracker, batch, timeout) : 
                new MultiRunner(task, tracker, batch);
            
            runner.Start(amount); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public static Tracker Schedule(
            this MultiTask task,
            ICollection amount,
            Tracker dependency,
            int batch = 1, 
            int timeout = 0) => Schedule(task, amount.Count, dependency, batch, timeout);
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask"/> to execute.</param>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> to take in milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public static Tracker Schedule(
            this MultiTask task, 
            int amount, 
            Tracker dependency, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(task, tracker, batch, timeout) : 
                new MultiRunner(task, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount);

            return tracker;
        }
    }
}