using System;

namespace Obscurum.TDT.Tests.Examples
{
    /// <summary>
    /// Class that represents an implementation of the <see cref="Routine"/> interfaces for testing purposes.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>1.0.0</version>
    internal sealed class ExampleRoutine : Routine
    {
        public bool setup;
        public int cycles;
        public bool crashed;
        public bool shutdown;

        protected override void Setup() => setup = true;

        protected override void Update()
        {
            cycles++;
            
            if (cycles <= 10) return;

            throw new TimeoutException();
        }

        protected override void Crash(Exception e) => crashed = true;

        protected override void Shutdown() => shutdown = true;
    }
}