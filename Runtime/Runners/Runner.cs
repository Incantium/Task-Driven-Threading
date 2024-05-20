﻿using System;
using System.Threading;
using Obscurum.TDT.Tasks;

namespace Obscurum.TDT.Runners
{
    /// <summary>
    /// Class able to run a <see cref="Task{T}"/> on a separate <see cref="Thread"/>.
    /// </summary>
    /// <typeparam name="T">The typing of the <see cref="Task{T}"/>.</typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    internal class Runner<T> : BaseRunner
    {
        private readonly Task<T> task;
        private readonly Tracker<T> tracker;
        
        /// <summary>
        /// Constructor to create a new <see cref="Runner{T}"/> for a <see cref="Task{T}"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task{T}"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker{T}"/> to update when completed.</param>
        internal Runner(Task<T> task, Tracker<T> tracker)
        {
            this.task = task;
            this.tracker = tracker;
        }

        /// <inheritdoc cref="BaseRunner.Start"/>
        public void Start()
        {
            var thread = new Thread(Run);
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
    /// Class able to run a <see cref="Task"/> on a separate <see cref="Thread"/>.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    internal class Runner : BaseRunner
    { 
        private readonly Task task;
        private readonly Tracker tracker;
        
        /// <summary>
        /// Constructor to create a new <see cref="Runner"/> for a <see cref="Task"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to run.</param>
        /// <param name="tracker">The <see cref="Tracker"/> to update when completed.</param>
        internal Runner(Task task, Tracker tracker)
        {
            this.task = task;
            this.tracker = tracker;
        }

        /// <inheritdoc cref="BaseRunner.Start"/>
        public void Start()
        {
            var thread = new Thread(Run);
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