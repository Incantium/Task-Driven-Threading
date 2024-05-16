using System;
using System.Collections;
using System.Threading;

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
        private int batch;
        
        /// <summary>
        /// Method called to execute the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask{T}"/></param>
        /// <returns>The return value of the <see cref="MultiTask{T}"/>.</returns>
        protected abstract T Execute(int i);
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(int amount, int batch = 1)
        {
            if (tracker != null) return tracker;

            tracker = new Tracker<T[]>();

            Start(amount, batch);

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(ICollection amount, int batch = 1)
        {
            if (tracker != null) return tracker;

            tracker = new Tracker<T[]>();

            Start(amount.Count, batch);

            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(int amount, Tracker dependency, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T[]>();
            
            dependency.success += () => Start(amount, batch);
                     
            return tracker;
        }

        /// <summary>
        /// Method to schedule the <see cref="MultiTask{T}"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask{T}"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask{T}"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker{T}"/> to keep track of the progress of the <see cref="MultiTask{T}"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask{T}"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker<T[]> Schedule(ICollection amount, Tracker dependency, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker<T[]>();
            
            dependency.success += () => Start(amount.Count, batch);
            
            return tracker;
        }

        private void Start(int amount, int batch)
        {
            this.amount = amount;
            this.batch = batch;
            
            tracker.amount = amount;
            result = new T[amount];
                        
            for (var i = 0; i < amount; i += batch)
            {
                var thread = new Thread(Run);
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
        private int batch;
        
        /// <summary>
        /// Method called to execute the <see cref="MultiTask"/>.
        /// </summary>
        /// <param name="i">The index of the current single task in the <see cref="MultiTask"/></param>
        protected abstract void Execute(int i);
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(int amount, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            Start(amount, batch);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(ICollection amount, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            Start(amount.Count, batch);
            
            return tracker;
        }
        
        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(int amount, Tracker dependency, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            dependency.success += () => Start(amount, batch);
                     
            return tracker;
        }

        /// <summary>
        /// Method to schedule the <see cref="MultiTask"/> to <see cref="Execute"/>.
        /// </summary>
        /// <param name="amount">The amount of single tasks to run in the <see cref="MultiTask"/>. This value is
        /// calculated dynamically.</param>
        /// <param name="dependency">The <see cref="Tracker"/> this <see cref="MultiTask"/> depends upon.</param>
        /// <param name="batch">The batch size or amount of single tasks run per thread. The amount of threads started
        /// is amount divided by the batch size, rounded up.</param>
        /// <returns>A <see cref="Tracker"/> to keep track of the progress of the <see cref="MultiTask"/>.
        /// </returns>
        /// <remarks>If the <see cref="MultiTask"/> has been scheduled before, this method will not schedule it again
        /// and only return the previous <see cref="tracker"/>.</remarks>
        public Tracker Schedule(ICollection amount, Tracker dependency, int batch = 1)
        {
            if (tracker != null) return tracker;
            
            tracker = new Tracker();
            
            dependency.success += () => Start(amount.Count, batch);
            
            return tracker;
        }
        
        private void Start(int amount, int batch)
        {
            this.amount = amount;
            this.batch = batch;
            
            tracker.amount = amount;
                        
            for (var i = 0; i < amount; i += batch)
            {
                var thread = new Thread(Run);
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