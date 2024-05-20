using System;
using System.Threading;

namespace Obscurum.TDT.Threads
{
    /// <summary>
    /// Class that represents a parameterized <see cref="thread"/> that will automatically <see cref="timeout"/> after a
    /// specified duration.
    /// </summary>
    /// <remarks>This class uses <see cref="Thread.Abort()"/> to kill the <see cref="thread"/> at timeout. This may
    /// produce unforeseen errors and cannot be used in .NET Core and .NET 5+.</remarks>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public class ParameterizedTimeoutThread
    {
        private readonly Thread thread;
        private readonly System.Timers.Timer timer;
        private readonly ParameterizedThreadStart method;
        private readonly int timeout;

        /// <summary>
        /// Event to get notified when the <see cref="thread"/> had timed out.
        /// </summary>
        public event Action<Exception> onTimeout;

        /// <summary>
        /// Constructor to create a new <see cref="ParameterizedTimeoutThread"/>.
        /// </summary>
        /// <param name="start">The method to run in the <see cref="thread"/>.</param>
        /// <param name="milliseconds">The maximum amount of milliseconds before the <see cref="thread"/> times out. It
        /// is set to a default of one minute.</param>
        public ParameterizedTimeoutThread(ParameterizedThreadStart start, int milliseconds = 60000)
        {
            thread = new Thread(Run);
            timer = new System.Timers.Timer(milliseconds);
            method = start;
            timeout = milliseconds;
            
            timer.Elapsed += (_, _) => Timeout();
        }

        /// <summary>
        /// Method to start the execution of the <see cref="thread"/> and the timeout <see cref="timer"/>.
        /// </summary>
        /// <param name="obj">The parameter of the <see cref="method"/>.</param>
        public void Start(object obj)
        {
            timer.Start();
            thread.Start(obj);
        }

        private void Run(object obj)
        {
            method.Invoke(obj);
            
            timer.Stop();
            timer.Dispose();
        }

        private void Timeout()
        {
            try
            {
                thread.Abort();
                onTimeout?.Invoke(new TimeoutException("The maximum allotted time of " + timeout + " milliseconds has been reached."));
            }
            catch (Exception e)
            {
                onTimeout?.Invoke(e);
            }
            
            timer.Dispose();
        }
    }
}