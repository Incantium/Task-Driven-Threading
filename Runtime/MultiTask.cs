using System;
using System.Collections;

namespace Obscurum.TDT
{
    /// <summary>
    /// Class that represents multiple likewise tasks with return type <see cref="T"/> that is run on separate threads.
    /// </summary>
    /// <typeparam name="T">The return typing of the task.</typeparam>
    /// <seealso cref="Task"/>
    /// <seealso cref="Task{T}"/>
    /// <seealso cref="MultiTask"/>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public abstract class MultiTask<T>
    {
        private T[] result;
        private Tracker<T[]> tracker;
        private int amount;
        private int batch = 1;
        private int timeout = 60000; // 1 minute
        
        /// <summary>
        /// Method called to execute the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask{T}"/></param>
        /// <returns>The return value of the <see cref="MultiTask{T}"/>.</returns>
        protected abstract T Execute(int i);
        
        /// <summary>
        /// Method to alter the allotted maximum time this <see cref="MultiTask{T}"/> may take. If this time is reached,
        /// the single task will throw an <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="milliseconds">The maximum allotted time for one single task.</param>
        /// <returns>The current <see cref="MultiTask{T}"/>.</returns>
        public MultiTask<T> Timeout(int milliseconds)
        {
            timeout = milliseconds;
            return this;
        }

        /// <summary>
        /// Method to alter the batch size of the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="amount">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the final amount divided by the batch size, rounded up.</param>
        public MultiTask<T> Batch(int amount)
        {
            batch = amount;
            return this;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(int amount)
        {
            if (tracker != null) return tracker;

            tracker = new Tracker<T[]>();

            Start(amount);

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(ICollection amount)
        {
            if (tracker != null) return tracker;

            tracker = new Tracker<T[]>();

            Start(amount.Count);

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(int amount, Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T[]>();
            
            dependency.dependency += () => Start(amount);
                     
            return tracker;
        }

        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(ICollection amount, Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T[]>();
            
            dependency.dependency += () => Start(amount.Count);
            
            return tracker;
        }
        
        private void Start(int amount)
        {
            this.amount = amount;
            tracker.amount = amount;

            result = new T[amount];
                        
            for (var i = 0; i < amount; i += batch)
            {
                var thread = new ParameterizedTimeoutThread(Run, timeout);
                thread.onTimeout += e => tracker.Complete(e);
                thread.Start(i);
            }
        }

        private void Run(object obj)
        {
            var value = (int) obj;
            
            for (var i = value; i < value + batch && i < amount; i++)
            {
                try
                { 
                    var instance = Execute(i);

                    result[i] = instance;
                    tracker.Complete(result);
                }
                catch (Exception e)
                {
                    tracker.Complete(e);
                }
            }
        }
    }

    /// <summary>
    /// Class that represents multiple likewise tasks with no return type that is run on separate threads.
    /// </summary>
    /// <seealso cref="Task{T}"/>
    /// <seealso cref="Task"/>
    /// <seealso cref="MultiTask{T}"/>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public abstract class MultiTask
    {
        private Tracker tracker;
        private int amount;
        private int batch = 1;
        private int timeout = 60000; // 1 minute
        
        /// <summary>
        /// Method called to execute the <see cref="MultiTask"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask"/></param>
        protected abstract void Execute(int i);
        
        /// <summary>
        /// Method to alter the allotted maximum time this <see cref="MultiTask"/> may take. If this time is reached,
        /// the single task will throw an <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="milliseconds">The maximum allotted time for one single task.</param>
        /// <returns>The current <see cref="MultiTask"/>.</returns>
        public MultiTask Timeout(int milliseconds)
        {
            timeout = milliseconds;
            return this;
        }
        
        /// <summary>
        /// Method to alter the batch size of the <see cref="MultiTask"/>.
        /// </summary>
        /// <param name="amount">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is the final amount divided by the batch size, rounded up.</param>
        public MultiTask Batch(int amount)
        {
            batch = amount;
            return this;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(int amount)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            Start(amount);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(ICollection amount)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            Start(amount.Count);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(int amount, Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            dependency.dependency += () => Start(amount);
                     
            return tracker;
        }

        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(ICollection amount, Tracker dependency)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            dependency.dependency += () => Start(amount.Count);
            
            return tracker;
        }
        
        private void Start(int amount)
        {
            this.amount = amount;
            tracker.amount = amount;
                        
            for (var i = 0; i < amount; i += batch)
            {
                var thread = new ParameterizedTimeoutThread(Run, timeout);
                thread.onTimeout += e => tracker.Complete(e);
                thread.Start(i);
            }
        }
        
        private void Run(object obj)
        {
            var value = (int) obj;

            for (var i = value; i < value + batch && i < amount; i++)
            {
                try
                {
                    Execute(i);
                    tracker.Complete();
                }
                catch (Exception e)
                {
                    tracker.Complete(e);
                }
            }
        }
    }
}