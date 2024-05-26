using System;
using System.Threading;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="Routine"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class RoutineTest
    {
        /// <summary>
        /// Test case if a <see cref="Routine"/> is <see cref="Routine.Setup"/> after <see cref="Routine.Start"/> is
        /// called.
        /// </summary>
        /// <method>This test will <see cref="Routine.Start"/> and <see cref="Routine.Stop"/> an
        /// <see cref="ExampleRoutine"/> one after another.</method>
        /// <expected>The <see cref="ExampleRoutine.setup"/> variable is set to true.</expected>
        /// <version>0.1.0</version>
        [Test, Repeat(10)]
        public void TestSetup()
        {
            // Arrange
            var routine = new ExampleRoutine();

            // Act
            routine.SetTps(60);
            routine.Start();
            routine.Stop();

            // Assert
            Assert.IsTrue(routine.setup);
        }

        /// <summary>
        /// Test case if a <see cref="Routine"/> can run multiple times before being <see cref="Routine.Shutdown"/>.
        /// </summary>
        /// <method>The <see cref="ExampleRoutine"/> is set with a <see cref="Routine.interval"/> of 100 milliseconds.
        /// It is then run for 450 milliseconds before being stopped.</method>
        /// <expected>The <see cref="ExampleRoutine.cycles"/> is expected to be 5 (as any <see cref="Routine"/> will
        /// start at 0 ms).</expected>
        /// <version>0.1.0</version>
        [Test, Repeat(10)]
        public void TestUpdate()
        {
            // Arrange
            var routine = new ExampleRoutine();
            
            // Act
            routine.SetInterval(100);
            routine.Start();
            
            Thread.Sleep(450);
            
            routine.Stop();

            // Assert
            Assert.AreEqual(5, routine.cycles);
        }

        /// <summary>
        /// Test case if a <see cref="Routine"/> can handle a <see cref="Routine.Crash"/> after an exception has been
        /// thrown.
        /// </summary>
        /// <method>This test will <see cref="Routine.Start"/> a <see cref="ExampleRoutine"/> with an
        /// <see cref="Routine.interval"/> of 20 milliseconds. The test will then wait for 400 milliseconds to give the
        /// <see cref="ExampleRoutine"/> to <see cref="Routine.Crash"/>.</method>
        /// <expected>The <see cref="ExampleRoutine.crashed"/> variable will contain a <see cref="TimeoutException"/>.
        /// </expected>
        /// <version>0.1.0</version>
        [Test, Repeat(10)]
        public void TestCrash()
        {
            // Arrange
            var expected = new TimeoutException();
            
            var routine = new ExampleRoutine();
            
            // Act
            routine.SetInterval(20);
            routine.Start();
            
            Thread.Sleep(400);
            
            routine.Stop();
            
            // Assert
            Assert.AreEqual(expected.GetType(), routine.crashed.GetType());
        }

        /// <summary>
        /// Test case if a <see cref="Routine"/> does <see cref="Routine.Shutdown"/> after being called to
        /// <see cref="Routine.Stop"/>.
        /// </summary>
        /// <method>This test will <see cref="Routine.Start"/> an <see cref="ExampleRoutine"/> with an
        /// <see cref="Routine.interval"/> of 50 milliseconds. It will then call it to <see cref="Routine.Stop"/>
        /// immediately afterwards. Then, the test will wait an additional of 100 milliseconds to give the
        /// <see cref="ExampleRoutine"/> to <see cref="Routine.Shutdown"/>.</method>
        /// <expected>The <see cref="ExampleRoutine.shutdown"/> variable will be set to true.</expected>
        /// <version>0.1.0</version>
        [Test, Repeat(10)]
        public void TestShutdown()
        {
            // Arrange
            var routine = new ExampleRoutine();
            
            // Act
            routine.SetInterval(50);
            routine.Start();
            routine.Stop();
            
            Thread.Sleep(100);
            
            // Assert
            Assert.IsTrue(routine.shutdown);
        }
    }
}