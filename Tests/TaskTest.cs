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
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/> will trigger the
        /// <see cref="Tracker.success"/> event when the <see cref="Task"/> is completed.</expected>
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
        /// <expected>The first task will trigger the start of the <see cref="Task"/>. Afterwards, the
        /// <see cref="Tracker"/> of the  <see cref="Task"/> triggers. This order will happen no matter the the
        /// scheduling order of the tasks.</expected>
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
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/> will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="Task"/> throws an exception.</expected>
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
        /// <expected>The <see cref="Tracker"/> of the <see cref="Task"/>will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="Task"/> has timed out after the allotted
        /// time.</expected>
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