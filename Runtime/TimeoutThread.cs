using System;
using System.Threading;

namespace Obscurum.TDT
{
    /// <summary>
    /// Class that represents a <see cref="thread"/> that will automatically <see cref="timeout"/> after a specified
    /// duration.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    public class TimeoutThread
    {
        private readonly Thread thread;
        private readonly System.Timers.Timer timer;
        private readonly ThreadStart method;
        private readonly int timeout;

        /// <summary>
        /// Event to get notified when the <see cref="thread"/> had timed out.
        /// </summary>
        public event Action<Exception> onTimeout;

        /// <summary>
        /// Constructor to create a new <see cref="TimeoutThread"/>.
        /// </summary>
        /// <param name="start">The method to run in the <see cref="thread"/>.</param>
        /// <param name="milliseconds">The maximum amount of milliseconds before the <see cref="thread"/> times out. It
        /// is set to a default of one minute.</param>
        public TimeoutThread(ThreadStart start, int milliseconds = 60000)
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
        public void Start()
        {
            timer.Start();
            thread.Start();
        }

        private void Run()
        {
            method.Invoke();
            
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