using System;
using System.Threading;

namespace Obscurum.TDT.Threads
{
    /// <summary>
    /// Class that represents a thread that will automatically <see cref="timeout"/> after a specified duration.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    public sealed class TimeoutThread
    {
        private readonly ThreadStart method;
        private readonly CancellationTokenSource source;
        private readonly int timeout;

        /// <summary>
        /// Event to get notified when the thread had timed out.
        /// </summary>
        public event Action<Exception> onTimeout;

        /// <summary>
        /// Constructor to create a new <see cref="TimeoutThread"/>.
        /// </summary>
        /// <param name="start">The method to run in the thread.</param>
        /// <param name="milliseconds">The maximum amount of milliseconds before the thread times out. It is set to a
        /// default of one minute.</param>
        public TimeoutThread(ThreadStart start, int milliseconds = 60000)
        {
            method = start;
            source = new CancellationTokenSource(milliseconds);
            timeout = milliseconds;
        }

        /// <summary>
        /// Method to start the execution of the thread.
        /// </summary>
        public void Start()
        {
            var token = source.Token;
            token.Register(Timeout);
            
            var task = new System.Threading.Tasks.Task(Run, token);
            
            task.Start();
        }

        private void Run()
        {
            method.Invoke();
            source.Dispose();
        }

        private void Timeout()
        {
            source.Dispose();
            onTimeout?.Invoke(new TimeoutException("The maximum allotted time of " + timeout + " milliseconds has been reached."));
        }
    }
}