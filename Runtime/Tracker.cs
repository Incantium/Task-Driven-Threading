using System;
using System.Collections.Generic;
using System.Timers;

namespace Obscurum.TDT
{
    /// <summary>
    /// Class to keep track of a <see cref="Task{T}"/> or <see cref="MultiTask{T}"/> for completion.
    /// </summary>
    /// <typeparam name="T">The <see cref="result"/> type of the <see cref="Task{T}"/> or <see cref="MultiTask{T}"/>.
    /// </typeparam>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public sealed class Tracker<T> : Tracker
    {
        private T outcome;
        
        private event Action<T> _result;
        /// <summary>
        /// Event to get notified when the <see cref="Tracker{T}"/> has received an <see cref="outcome"/>.
        /// </summary>
        /// <remarks>If the <see cref="Tracker{T}"/> has already completed, the added event will be immediately called.
        /// </remarks>
        public event Action<T> result
        {
            add
            {
                lock (key)
                {
                    if (final) value.Invoke(outcome);
                    else _result += value;
                }
            }
            remove => _result -= value;
        }

        internal void Complete(T result)
        {
            lock (key)
            {
                Increment();
                Notify();
                
                if (!complete) return;
                
                outcome = result;
                _result?.Invoke(result);

                Final();
            }
        }
    }
    
    /// <summary>
    /// Class to keep track of a <see cref="Task"/> or <see cref="MultiTask"/> for completion.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public class Tracker
    {
        private readonly List<Exception> exceptions = new();
        private int done;
        internal readonly object key = new();
        internal bool complete;
        internal int amount = 1;
        internal bool final;
        
        /// <summary>
        /// Property indicating the progression of the <see cref="Tracker"/> from 0 to 100%.
        /// </summary>
        public float percentage => 100f * done / amount;
        
        private event Action _success; 
        /// <summary>
        /// Event to get notified when the <see cref="Tracker"/> has completed. This event will be called, even when an
        /// some <see cref="exceptions"/> have triggered.
        /// </summary>
        /// <remarks>If the <see cref="Tracker"/> has already completed, the added event will be immediately called.
        /// </remarks>
        public event Action success
        {
            add
            {
                lock (key)
                {
                    if (final) value.Invoke();
                    else _success += value;
                }
            }
            remove => _success -= value;
        }
        
        private event Action<List<Exception>> _exception;
        /// <summary>
        /// Event to get notified when the <see cref="Tracker"/> has received an exception.
        /// </summary>
        /// <remarks>If the <see cref="Tracker"/> has already completed, the added event will be immediately called.
        /// </remarks>
        public event Action<List<Exception>> exception
        {
            add
            {
                lock (key)
                {
                    if (final && exceptions.Count != 0) value.Invoke(exceptions);
                    else _exception += value;
                }
                
            }
            remove => _exception -= value;
        }

        private event Action _dependency;
        internal event Action dependency
        {
            add
            {
                lock (key)
                {
                    if (final) value.Invoke();
                    else _dependency += value;
                }
            }
            remove => _dependency -= value;
        }

        /// <summary>
        /// Method to wait for the <see cref="Tracker"/> to be completed. This method will continue after everything has
        /// completed and all events has been called. For more safety, it is advised to use <see cref="Wait(int)"/>.
        /// </summary>
        /// <remarks>This method will stall the current thread for executing until the <see cref="Tracker"/> has been
        /// completed.</remarks>
        /// <seealso cref="Wait(int)"/>
        public void Wait()
        {
            while (!final) {}
        }
        
        /// <summary>
        /// Method to wait for the <see cref="Tracker"/> to be completed. This method will continue after everything has
        /// completed and all events has been called, or the allotted time has run out.
        /// </summary>
        /// <param name="milliseconds">The maximum amount of milliseconds the <see cref="Tracker"/> will wait for a
        /// resolution.</param>
        /// <exception cref="TimeoutException">Thrown when the allotted time has run out without a resolution.
        /// </exception>
        public void Wait(int milliseconds)
        {
            var elapsed = false;
            var timer = new Timer(milliseconds);

            timer.Elapsed += (_, _) => elapsed = true;
            timer.Start();

            while (!final)
            {
                if (elapsed) throw new TimeoutException();
            }
        }

        internal void Complete(Exception exception = null)
        {
            lock (key)
            { 
                if (exception != null) exceptions.Add(exception);
                
                Increment();
                Notify();
                
                if (!complete) return;

                Final();
            }
        }

        internal void Increment()
        {
            done = Math.Min(done + 1, amount);
            complete = done >= amount;
        }

        internal void Notify()
        {
            if (!complete) return;
            
            if (exceptions.Count != 0) _exception?.Invoke(exceptions);
            _success?.Invoke();
        }

        internal void Final()
        {
            final = true;
            _dependency?.Invoke();
        }
    }
}