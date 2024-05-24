using System;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="Task"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class TaskTest
    {
        /// <summary>
        /// Test case if a <see cref="Task"/> can be started and completed with a <see cref="Tracker.success"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <method>This task will schedule a <see cref="ExampleTask"/> as a <see cref="Task"/>. It will then
        /// <see cref="Tracker.Wait(int)"/> for its completion.</method>
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/> will trigger the <see cref="Tracker.success"/>
        /// event when the all the single tasks of the <see cref="Task"/> are completed.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTask([Values(0, 1000)] int timeout)
        {
            // Arrange
            var complete = false;

            Task task = new ExampleTask();
            
            // Act
            var tracker = task.Schedule(timeout);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }

        /// <summary>
        /// Test case if a <see cref="Task"/> can be dependent upon another task. This test will check if the completion
        /// of one task can trigger the activation of the <see cref="Task"/>.
        /// </summary>
        /// <method>This task will schedule two tasks at the same time, the first with a delay of 100 milliseconds, and
        /// the second with a dependency on the first. The test will then <see cref="Tracker.Wait(int)"/> for both tasks
        /// to be completed.</method>
        /// <expected>This order of task <see cref="Tracker.success"/> will happen in the order of dependency, no matter
        /// the scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestDependency()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";

            Task task = new ExampleTask();
            
            // Act
            var tracker1 = task.Schedule();
            var tracker2 = task.Schedule(tracker1);

            tracker1.success += () => actual += "Hello ";
            tracker2.success += () => actual += "World!";
            
            tracker2.Wait(1000);
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test case if a <see cref="Task"/> can be ended in a <see cref="Tracker.exception"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <method>This test will start a <see cref="ExceptionTask"/> as a <see cref="Task"/>. Then, the test will
        /// <see cref="Tracker.Wait(int)"/> for the task to end.
        /// </method>
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/> will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="Task"/> throws any one exception.</expected>
        /// <version>1.0.0</version>
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
        
        /// <summary>
        /// Test case if a <see cref="Task"/> can timeout.
        /// </summary>
        /// <method>This test will create a <see cref="LagTask"/> as a <see cref="Task"/> with a duration of 500
        /// milliseconds. It will then schedule it for a maximum time of 100 milliseconds. It then waits for the task to
        /// end. </method>
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/>will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="Task"/> has timed out after the allotted
        /// time run out.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTimeout()
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
    }
}