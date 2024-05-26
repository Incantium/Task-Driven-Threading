using System;
using Obscurum.TDT.Threads;

namespace Obscurum.TDT.Runners
{
    /// <summary>
    /// Class able to run a <see cref="MultiTask{T}"/> on a separate <see cref="ParameterizedTimeoutThread"/>.
    /// </summary>
    /// <typeparam name="T">The typing of the <see cref="MultiTask{T}"/>.</typeparam>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal class MultiTimeoutRunner<T> : BaseMultiRunner
    {
        private readonly MultiTask<T> task;
        private readonly Tracker<T[]> tracker;
        private readonly int batch;
        private readonly int timeout;
        
        private T[] result;
        private int amount;
        
        /// <summary>
        /// Constructor to create a new <see cref="MultiTimeoutRunner{T}"/> for a <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask{T}"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker{T}"/> to update when completed.</param>
        /// <param name="batch">The amount of tasks for the <see cref="MultiTask{T}"/>.</param>
        /// <param name="timeout">The maximum amount of milliseconds this <see cref="MultiTask{T}"/> may take.</param>
        internal MultiTimeoutRunner(MultiTask<T> task, Tracker<T[]> tracker, int batch = 1, int timeout = 60000)
        {
            this.task = task;
            this.tracker = tracker;
            this.batch = batch;
            this.timeout = timeout;
        }
        
        /// <inheritdoc cref="BaseMultiRunner.Start"/>
        public void Start(int amount)
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
                    var instance = task.Execute(i);
                    
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
    /// Class able to run a <see cref="MultiTask"/> on a separate <see cref="ParameterizedTimeoutThread"/>.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal class MultiTimeoutRunner : BaseMultiRunner
    {
        private readonly MultiTask task;
        private readonly Tracker tracker;
        private readonly int batch;
        private readonly int timeout;
        
        private int amount;
        
        /// <summary>
        /// Constructor to create a new <see cref="MultiTimeoutRunner"/> for a <see cref="MultiTask"/>.
        /// </summary>
        /// <param name="task">The <see cref="MultiTask"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker"/> to update when completed.</param>
        /// <param name="batch">The amount of tasks for the <see cref="MultiTask"/>.</param>
        /// <param name="timeout">The maximum amount of milliseconds this <see cref="MultiTask"/> may take.</param>
        internal MultiTimeoutRunner(MultiTask task, Tracker tracker, int batch = 1, int timeout = 60000)
        {
            this.task = task;
            this.tracker = tracker;
            this.batch = batch;
            this.timeout = timeout;
        }
        
        /// <inheritdoc cref="BaseMultiRunner.Start"/>
        public void Start(int amount)
        {
            this.amount = amount;
            tracker.amount = amount;
            
            for (var i = 0; i < amount; i += batch)
            {
                var thread = new ParameterizedTimeoutThread(Run, timeout);
                thread.onTimeout += e => tracker.Complete(e, batch);
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
                    task.Execute(i);
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