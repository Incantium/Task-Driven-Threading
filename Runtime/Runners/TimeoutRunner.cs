using System;
using Obscurum.TDT.Threads;

namespace Obscurum.TDT.Runners
{
    /// <summary>
    /// Class able to run a <see cref="Task{T}"/> on a separate <see cref="TimeoutThread"/>.
    /// </summary>
    /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal class TimeoutRunner<T> : BaseRunner
    {
        private readonly Task<T> task;
        private readonly Tracker<T> tracker;
        private readonly int timeout;
        
        /// <summary>
        /// Constructor to create a new <see cref="TimeoutRunner{T}"/> for a <see cref="Task{T}"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task{T}"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker{T}"/> to update when completed.</param>
        /// <param name="timeout">The maximum amount of milliseconds this <see cref="Task{T}"/> may take.</param>
        internal TimeoutRunner(Task<T> task, Tracker<T> tracker, int timeout)
        {
            this.task = task;
            this.tracker = tracker;
            this.timeout = timeout;
        }

        /// <inheritdoc cref="BaseRunner.Start"/>
        public void Start()
        {
            var thread = new TimeoutThread(Run, timeout);
            thread.onTimeout += e => tracker.Complete(e);
            thread.Start();
        }

        private void Run()
        {
            try
            {
                var result = task.Execute();
                tracker.Complete(result);
            }
            catch (Exception e)
            {
                tracker.Complete(e);
            }
        }
    }
    
    /// <summary>
    /// Class able to run a <see cref="Task"/> on a separate <see cref="TimeoutThread"/>.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal class TimeoutRunner : BaseRunner
    { 
        private readonly Task task;
        private readonly Tracker tracker;
        private readonly int timeout;
        
        /// <summary>
        /// Constructor to create a new <see cref="TimeoutRunner"/> for a <see cref="Task"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker"/> to update when completed.</param>
        /// <param name="timeout">The maximum amount of milliseconds this <see cref="Task"/> may take.</param>
        internal TimeoutRunner(Task task, Tracker tracker, int timeout)
        {
            this.task = task;
            this.tracker = tracker;
            this.timeout = timeout;
        }

        /// <inheritdoc cref="BaseRunner.Start"/>
        public void Start()
        {
            var thread = new TimeoutThread(Run, timeout);
            thread.onTimeout += e => tracker.Complete(e);
            thread.Start();
        }

        private void Run()
        {
            try
            {
                task.Execute();
                tracker.Complete();
            }
            catch (Exception e)
            {
                tracker.Complete(e);
            }
        }
    }
}