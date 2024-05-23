using System.Collections;
using Obscurum.TDT.Runners;

namespace Obscurum.TDT
{
    /// <summary>
    /// Interface that represents multiple likewise tasks with return type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public interface MultiTask<T>
    {
        /// <summary>
        /// Method called to execute the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask{T}"/>.</param>
        /// <returns>The return value of the <see cref="MultiTask{T}"/>.</returns>
        T Execute(int i);
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public Tracker<T[]> Schedule(
            int amount, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(this, tracker, batch, timeout) : 
                new MultiRunner<T>(this, tracker, batch);
            
            runner.Start(amount); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public Tracker<T[]> Schedule(
            ICollection amount,
            int batch = 1,
            int timeout = 0) 
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(this, tracker, batch, timeout) : 
                new MultiRunner<T>(this, tracker, batch);
            
            runner.Start(amount.Count); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public Tracker<T[]> Schedule(
            int amount, 
            Tracker dependency, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(this, tracker, batch, timeout) : 
                new MultiRunner<T>(this, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask{T}"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        public Tracker<T[]> Schedule(
            ICollection amount,
            Tracker dependency,
            int batch = 1, 
            int timeout = 0) 
        {
            var tracker = new Tracker<T[]>();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner<T>(this, tracker, batch, timeout) : 
                new MultiRunner<T>(this, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount.Count);

            return tracker;
        }
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
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public Tracker Schedule(
            int amount, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(this, tracker, batch, timeout) : 
                new MultiRunner(this, tracker, batch);
            
            runner.Start(amount); 
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public Tracker Schedule(
            ICollection amount,
            int batch = 1, 
            int timeout = 0) 
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(this, tracker, batch, timeout) : 
                new MultiRunner(this, tracker, batch);
            
            runner.Start(amount.Count); 

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public Tracker Schedule(
            int amount, 
            Tracker dependency, 
            int batch = 1, 
            int timeout = 0)
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(this, tracker, batch, timeout) : 
                new MultiRunner(this, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule a <see cref="MultiTask"/> for execution.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the <see cref="amount"/> divided by the <see cref="batch"/> size, rounded up.</param>
        /// <param name="timeout">The maximum allotted time for a <see cref="batch"/> of single tasks to take in
        /// milliseconds.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.</returns>
        public Tracker Schedule(
            ICollection amount,
            Tracker dependency,
            int batch = 1, 
            int timeout = 0) 
        {
            var tracker = new Tracker();
            
            BaseMultiRunner runner = timeout > 0 ? 
                new MultiTimeoutRunner(this, tracker, batch, timeout) : 
                new MultiRunner(this, tracker, batch);
            
            dependency.dependency += () => runner.Start(amount.Count);

            return tracker;
        }
    }
}