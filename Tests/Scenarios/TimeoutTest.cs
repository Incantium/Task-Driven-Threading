using System;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests.Scenarios
{
    internal sealed class TimeoutTest
    {
        [Test, Repeat(10)]
        public void TestTaskTimeout()
        {
            // Arrange
            var expected = new TimeoutException();
            Exception actual = null;

            Task task = new LagTask(500);

            // Assert
            var tracker = task.Schedule(100);
            tracker.exception += e => actual = e[0];
            
            tracker.Wait(1000);

            // Act
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
        
        [Test, Repeat(10)]
        public void TestReturnTaskTimeout()
        {
            // Arrange
            var expected = new TimeoutException();
            Exception actual = null;

            Task<string> task = new LagTask(500);

            // Assert
            var tracker = task.Schedule(100);
            tracker.exception += e => actual = e[0];
            
            tracker.Wait(1000);

            // Act
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
        
        [Test, Repeat(10)]
        public void TestMultiTaskTimeout()
        {
            // Arrange
            var expected = new TimeoutException();
            Exception actual = null;

            MultiTask task = new LagTask(500);

            // Assert
            var tracker = task.Schedule(2, 2, 100);
            tracker.exception += e => actual = e[0];
            
            tracker.Wait(1000);

            // Act
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
        
        [Test, Repeat(10)]
        public void TestReturnMultiTaskTimeout()
        {
            // Arrange
            var expected = new TimeoutException();
            Exception actual = null;

            MultiTask<string> task = new LagTask(500);

            // Assert
            var tracker = task.Schedule(2, 2, 100);
            tracker.exception += e => actual = e[0];
            
            tracker.Wait(1000);

            // Act
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}