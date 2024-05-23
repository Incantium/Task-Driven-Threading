using System.Threading;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    internal sealed class RoutineTest
    {
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

        [Test, Repeat(10)]
        public void TestCrash()
        {
            // Arrange
            var routine = new ExampleRoutine();
            
            // Act
            routine.SetInterval(20);
            routine.Start();
            
            Thread.Sleep(400);
            
            routine.Stop();
            
            // Assert
            Assert.IsTrue(routine.crashed);
        }

        [Test, Repeat(10)]
        public void TestShutdown()
        {
            // Arrange
            var routine = new ExampleRoutine();
            
            // Act
            routine.SetInterval(100);
            routine.Start();
            routine.Stop();
            
            Thread.Sleep(200);
            
            // Assert
            Assert.IsTrue(routine.shutdown);
        }
    }
}