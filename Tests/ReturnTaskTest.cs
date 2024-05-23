using System;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="Task{T}"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class ReturnTaskTest
    {
        /// <summary>
        /// Test case if a <see cref="Task{T}"/> can be started and completed with a <see cref="Tracker{T}.result"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="Task{T}"/> will trigger the
        /// <see cref="Tracker{T}.result"/> event when the <see cref="Task{T}"/> is completed with the expected result.
        /// </expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTask([Values(0, 1000)] int timeout)
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";

            Task<string> task = new ExampleTask(expected);

            // Act
            var tracker = task.Schedule(timeout);
            tracker.result += result => actual = result;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="Task{T}"/> can be dependent upon another task. This test will check if the
        /// completion of one task can trigger the activation of the <see cref="Task{T}"/>.
        /// </summary>
        /// <expected>The first task will trigger the start of the <see cref="Task{T}"/>. Afterwards, the
        /// <see cref="Tracker{T}"/> of the  <see cref="MultiTask{T}"/> triggers. This order will happen no matter the
        /// the scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestDependency()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";

            Task<string> task1 = new ExampleTask("Hello ");
            Task<string> task2 = new ExampleTask("World!");

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(tracker1);
            
            tracker1.result += result => actual += result;
            tracker2.result += result => actual += result;
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="Task{T}"/> can be ended in a <see cref="Tracker{T}.exception"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="Task{T}"/> will trigger the
        /// <see cref="Tracker{T}.exception"/> event when the <see cref="Task{T}"/> throws an exception.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestException([Values(0, 1000)] int timeout)
        {
            // Arrange
            var expected = new Exception();
            Exception actual = null;

            Task<string> task = new ExceptionTask(expected);

            // Act
            var tracker = task.Schedule(timeout);
            tracker.exception += exception => actual = exception[0];
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="Task{T}"/> can timeout.
        /// </summary>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="Task{T}"/>will trigger the
        /// <see cref="Tracker{T}.exception"/> event when the <see cref="Task{T}"/> has timed out after the allotted
        /// time.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTimeout()
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
    }
}