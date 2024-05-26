using System;
using System.Threading;

namespace Obscurum.TDT
{
    /// <summary>
    /// <para>A <see cref="Routine"/> transforms any class into a continues runnable <see cref="timer"/>. Implement the
    /// endpoints, like <see cref="Setup"/> and <see cref="Update"/>, to keep the <see cref="Routine"/> running.</para>
    ///
    /// <para>The <see cref="Routine"/> will not automatically stop, not even when the main thread is stopped. Only a
    /// <see cref="Crash"/> or the <see cref="Stop"/> method will stop the <see cref="Routine"/>. In both cases, the
    /// <see cref="Shutdown"/> method will be called before termination.</para>
    /// 
    /// <para>The <see cref="Routine"/> will call the <see cref="Update"/> function each tick specified by its clock
    /// speed. If no clock speed is specified, the <see cref="Routine"/> will run at 60 ticks per second, or within an
    /// <see cref="interval"/> of about 17 seconds.</para>
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    public class Routine
    {
        private Timer timer;
        private int interval = 1000 / 60;
        private bool running;

        /// <summary>
        /// Method endpoint called to setup the <see cref="Routine"/>. This method is called once after the
        /// <see cref="Routine"/> is <see cref="Start"/>. Implement this method to setup the whole <see cref="Routine"/>
        /// before the continuous <see cref="Update"/>.
        /// </summary>
        /// <seealso cref="SetTps"/>
        protected virtual void Setup() {}
        
        /// <summary>
        /// Method endpoint called to update the <see cref="Routine"/>. This method is called each
        /// <see cref="interval"/>. Implement this method to continuous update after its <see cref="Setup"/>.
        /// </summary>
        protected virtual void Update() {}
        
        /// <summary>
        /// Method endpoint called when an <see cref="Exception"/> has occurred in the <see cref="Routine"/>. Implement
        /// this method to gracefully resolve the <see cref="Exception"/> thrown or <see cref="Start"/> the
        /// <see cref="Routine"/> again.
        /// </summary>
        /// <param name="e">The <see cref="Exception"/> thrown.</param>
        protected virtual void Crash(Exception e) {}
        
        /// <summary>
        /// Endpoint called to after a <see cref="Stop"/> or <see cref="Crash"/> to shutdown the <see cref="Routine"/>.
        /// Implement this method to shutdown the <see cref="Routine"/> by releasing all necessary assets and data.
        /// </summary>
        protected virtual void Shutdown() {}
        
        /// <summary>
        /// Method to start the <see cref="Routine"/>.This will call <see cref="Setup"/> once. Afterwards, this method
        /// will call <see cref="Update"/> each <see cref="interval"/> specified. This method will make the
        /// <see cref="Routine"/> not stop until the <see cref="Routine"/> is called to <see cref="Stop"/> or a
        /// <see cref="Crash"/> has occurred.
        /// </summary>
        /// <remarks>Once the <see cref="Routine"/> has <see cref="Start"/>, subsequently calling this method will do
        /// nothing.
        /// </remarks>
        public void Start()
        {
            if (running) return;
            
            Setup();
            timer = new Timer(Run, null, 0, interval);
            running = true;
        }

        /// <summary>
        /// Method to stop the <see cref="Routine"/>. This will stop after completing its last <see cref="Update"/> and
        /// the <see cref="Shutdown"/>.
        /// </summary>
        public void Stop() => running = false;
        
        private void Run(object _)
        {
            try
            {
                if (running)
                {
                    Update();
                    return;
                }
            }
            catch (Exception e)
            {
                Crash(e);
            }
            
            Shutdown();
            timer.Dispose();
        }
        
        /// <summary>
        /// Method to alter the <see cref="interval"/> of the <see cref="Routine"/> in milliseconds.
        /// </summary>
        /// <param name="milliseconds">The new clock speed in amount of milliseconds between <see cref="Update"/> calls.
        /// </param>
        /// <remarks>This new <see cref="interval"/> will only go into effect after a fresh <see cref="Start"/>.
        /// </remarks>
        public void SetInterval(int milliseconds) => interval = milliseconds;

        /// <summary>
        /// Method to alter the <see cref="interval"/> of the <see cref="Routine"/> in ticks per second.
        /// </summary>
        /// <param name="tps">The new clock speed in amount of ticks per second between <see cref="Update"/> calls.
        /// </param>
        /// <remarks>This new <see cref="interval"/> will only go into effect after a fresh <see cref="Start"/>.
        /// </remarks>
        public void SetTps(int tps) => SetInterval(1000 / tps);
        
        public override string ToString() => "Routine{" + 
                                             "Current status=" + (running ? "running" : "idle") + ", " + 
                                             "interval=" + interval + " milliseconds, " + 
                                             "tps=" + 1000 / interval + " ticks per second" + 
                                             "}";
    }
}