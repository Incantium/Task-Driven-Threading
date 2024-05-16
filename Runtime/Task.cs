using System;
using System.Threading;

namespace Obscurum.TDT
{
    /// <summary>
    /// Class that represents a single task with return type <see cref="T"/> that is run on a separate thread.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <seealso cref="Task"/>
    /// <seealso cref="MultiTask{T}"/>
    /// <seealso cref="MultiTask"/>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public abstract class Task<T>
    {
        private Tracker<T> tracker;
        
        /// <summary>
        /// Method called to execute the <see cref="Task{T}"/>.
        /// </summary>
        /// <returns>The return value of the <see cref="Task{T}"/>.</returns>
        protected abstract T Execute();
        
        /// <summary>
        /// Method to schedule the <see cref="Task{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        /// <remarks>If the <see cref="Task{T}"/> has been scheduled before, this method will not schedule it again and
        /// only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T> Schedule()
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T>();
            
            Start();
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="Task{T}"/> to <see cref="Execute"/> after the <see cref="dependency"/> has
        /// been resolved.
        /// </summary>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task{T}"/> depends upon.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="Task{T}"/>.</returns>
        /// <remarks>If the <see cref="Task{T}"/> has been scheduled before, this method will not schedule it again and
        /// only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T> Schedule(Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T>();

            dependency.success += Start;

            return tracker;
        }

        private void Start()
        {
            var thread = new Thread(Run);
            thread.Start();
        }
        
        private void Run()
        {
            try
            {
                var result = Execute();
                tracker.Complete(result);
            }
            catch (Exception e)
            {
                tracker.Complete(e);
            }
        }
    }
    
    /// <summary>
    /// Class that represents a single task with no return type that is run on a separate thread.
    /// </summary>
    /// <seealso cref="Task{T}"/>
    /// <seealso cref="MultiTask{T}"/>
    /// <seealso cref="MultiTask"/>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public abstract class Task
    {
        private Tracker tracker;
        
        /// <summary>
        /// Method called to execute the <see cref="Task"/>.
        /// </summary>
        protected abstract void Execute();
        
        /// <summary>
        /// Method to schedule the <see cref="Task"/> to <see cref="Execute"/>.
        /// </summary>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        /// <remarks>If the <see cref="Task"/> has been scheduled before, this method will not schedule it again and
        /// only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule()
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            Start();

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="Task"/> to <see cref="Execute"/> after the <see cref="dependency"/> has
        /// been resolved.
        /// </summary>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="Task"/> depends upon.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="Task"/>.</returns>
        /// <remarks>If the <see cref="Task"/> has been scheduled before, this method will not schedule it again and
        /// only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();

            dependency.success += Start;

            return tracker;
        }
        
        private void Start()
        {
            var thread = new Thread(Run);
            thread.Start();
        }
        
        private void Run()
        {
            try
            {
                Execute();
                tracker.Complete();
            }
            catch (Exception e)
            {
                tracker.Complete(e);
            }
        }
    }
}