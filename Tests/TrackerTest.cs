#pragma warning disable CS0618 // Type or member is obsolete

using System.Threading;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="Tracker"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class TrackerTest
    {
        /// <summary>
        /// Test case if the <see cref="Tracker.percentage"/> updates correctly over the execution of a task.
        /// </summary>
        /// <method>This test uses a <see cref="MultiTask"/> in the form of a <see cref="LagTask"/>, with the first
        /// single task taking 100 milliseconds, and the second taking 200 milliseconds. This <see cref="MultiTask{T}"/>
        /// is scheduled and periodically checked for its <see cref="Tracker.percentage"/> progression.</method>
        /// <expected>At the beginning of the <see cref="MultiTask"/>, it is expected the progression to be 0%. After
        /// 100+ milliseconds, the <see cref="Tracker.percentage"/> is expected to be 50%. And at the end of the
        /// <see cref="MultiTask"/>, it is expected to be 100%.</expected>
        /// <version>0.1.0</version>
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
        
        /// <summary>
        /// Test case if two parallel tasks can be tracked by joining the individual <see cref="Tracker"/>.
        /// </summary>
        /// <param name="delay1">The milliseconds the first task is delayed at its execution.</param>
        /// <param name="delay2">The milliseconds the second task is delayed at its execution.</param>
        /// <method>This test creates two <see cref="LagTask"/>, both with a delay. These two tasks are then scheduled
        /// at the same moment. The unique <see cref="Tracker"/> of both tasks are <see cref="Tracker.Join"/> to create
        /// one single <see cref="Tracker"/>. This test then waits for the completion of this third
        /// <see cref="Tracker"/>.</method>
        /// <expected>It is expected that the <see cref="Tracker"/> created from two other tasks will update accordingly
        /// and create a <see cref="Tracker.success"/> event when both tasks have completed, no matter the order of task
        /// completion.</expected>
        /// <version>0.1.0</version>
        [Test, Repeat(10)]
        public void TestJoin([Values(0, 200)] int delay1, [Values(0, 200)] int delay2)
        {
            // Arrange
            var complete = false;

            Task task1 = new LagTask(delay1);
            Task task2 = new LagTask(delay2);

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