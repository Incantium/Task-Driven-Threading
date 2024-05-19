using System;
using System.Threading;
using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class TaskWithLag : Task
    {
        protected override void Execute() => Thread.Sleep(200);

        [Test]
        public void TestTaskWithTimeout()
        {
            // Arrange
            var expected = new TimeoutException();
            Exception actual = null;
            
            var task = new TaskWithLag();
            
            // Act
            var tracker = task.Timeout(50).Schedule();

            tracker.exception += list => actual = list[0];
            tracker.Wait();

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
        
        [Test, Repeat(10)]
        public void TestTaskWithWaitTimeout()
        {
            // Arrange
            var task = new TaskWithLag();
            
            // Act
            var tracker = task.Schedule();

            // Assert
            Assert.Throws<TimeoutException>(() => tracker.Wait(50));
        }
        
        [Test, Repeat(10)]
        public void TestTaskPercentage()
        {
            // Arrange
            var task = new TaskWithLag();
            
            // Act
            var tracker = task.Schedule();

            var start = tracker.percentage;
            
            tracker.Wait();

            var end = tracker.percentage;

            //Assert
            Assert.AreEqual(0f, start);
            Assert.AreEqual(100f, end);
        }
    }
}