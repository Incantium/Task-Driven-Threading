using System;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    public class TaskTest
    {
        [Test, Repeat(10)]
        public void TestTask([Values(0, 1000)] int timeout)
        {
            // Arrange
            var complete = false;

            Task task = new ExampleTask("");
            
            // Act
            var tracker = task.Schedule(timeout);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }

        [Test, Repeat(10)]
        public void TestDependency()
        {
            // Arrange
            var complete = false;

            Task task = new ExampleTask("");
            
            // Act
            var tracker1 = task.Schedule();
            var tracker2 = task.Schedule(tracker1);
            tracker2.success += () => complete = true;
            
            tracker2.Wait(1000);
            
            // Assert
            Assert.IsTrue(complete);
        }

        [Test, Repeat(10)]
        public void TestException([Values(0, 1000)] int timeout)
        {
            // Arrange
            var expected = new Exception();
            Exception actual = null;

            Task task = new ExceptionTask(expected);

            // Act
            var tracker = task.Schedule(timeout);
            tracker.exception += exception => actual = exception[0];
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}