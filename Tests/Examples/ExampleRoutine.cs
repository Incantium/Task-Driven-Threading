using System;

namespace Obscurum.TDT.Tests.Examples
{
    /// <summary>
    /// Class that represents an implementation of the <see cref="Routine"/> class for testing purposes.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal sealed class ExampleRoutine : Routine
    {
        /// <summary>
        /// True if the <see cref="ExampleRoutine"/> has been <see cref="Routine.Setup"/>, otherwise false.
        /// </summary>
        public bool setup;
        
        /// <summary>
        /// The amount of <see cref="Routine.Update"/> cycles this <see cref="ExampleRoutine"/> has made.
        /// </summary>
        public int cycles;
        
        /// <summary>
        /// The <see cref="Exception"/> this <see cref="ExampleRoutine"/> has thrown at <see cref="Routine.Crash"/>.
        /// </summary>
        public Exception crashed;
        
        /// <summary>
        /// True if the <see cref="ExampleRoutine"/> has been <see cref="Routine.Shutdown"/>, otherwise false.
        /// </summary>
        public bool shutdown;

        protected override void Setup() => setup = true;

        protected override void Update()
        {
            cycles++;
            
            if (cycles <= 10) return;

            throw new TimeoutException();
        }

        protected override void Crash(Exception e) => crashed = e;

        protected override void Shutdown() => shutdown = true;
    }
}