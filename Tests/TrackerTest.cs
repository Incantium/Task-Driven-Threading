using System.Threading;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    internal sealed class TrackerTest
    {
        [Test, Repeat(10)]
        public void TestPercentage()
        {
            // Arrange
            MultiTask task = new LagTask(100);

            // Act
            var tracker = task.Schedule(2);

            var begin = tracker.percentage;
           
            Thread.Sleep(150);

            var middle = tracker.percentage;
           
            tracker.Wait(1000);

            var end = tracker.percentage;

            // Assert
            Assert.AreEqual(0f, begin);
            Assert.AreEqual(50f, middle);
            Assert.AreEqual(100f, end);
        }
        
        [Test, Repeat(10)]
        public void TestJoin([Values(0, 200)] int timeout1, [Values(0, 200)] int timeout2)
        {
            // Arrange
            var complete = false;

            Task task1 = new LagTask(timeout1);
            Task task2 = new LagTask(timeout2);

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule();

            var tracker3 = tracker1.Join(tracker2);
            tracker3.success += () => complete = true;
            tracker3.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }
    }
}