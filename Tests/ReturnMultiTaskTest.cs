using System;
using System.Collections.Generic;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="MultiTask{T}"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class ReturnMultiTaskTest
    {
        private static readonly Random RANDOM = new();
        
        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can be started and completed with a
        /// <see cref="Tracker{T}.success"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <param name="batch">Test at different batch sizes.</param>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="MultiTask{T}"/> will trigger the
        /// <see cref="Tracker{T}.success"/> event when the all the single tasks of the <see cref="MultiTask{T}"/> are
        /// completed.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestMultiTask([Range(1, 5)] int batch, [Values(0, 1000)] int timeout)
        {
            // Arrange
            const string i = "i";
            
            var expected = new[] { i, i, i, i, i };
            string[] actual = null;

            MultiTask<string> task = new ExampleTask(i);

            // Act
            var tracker = task.Schedule(5, batch, timeout);
            tracker.result += result => actual = result;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can be started and completed with a
        /// <see cref="Tracker{T}.success"/> with a random amount at runtime.
        /// </summary>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="MultiTask{T}"/> will trigger the
        /// <see cref="Tracker{T}.success"/> event when the all the single tasks of the <see cref="MultiTask{T}"/> are
        /// completed.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestRandomMultiTask()
        {
            // Arrange
            const string check = "i";
            var amount = RANDOM.Next(2, 10);

            var expected = new string[amount];
            for (var i = 0; i < amount; i++) expected[i] = check;
            
            string[] actual = null;
            
            MultiTask<string> task = new ExampleTask(check);

            // Act
            var tracker = task.Schedule(expected, 2);
            tracker.result += result => actual = result;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can be dependent upon another task. This test will check if the
        /// completion of one task can trigger the activation of the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <expected>The first task will trigger the start of the <see cref="MultiTask{T}"/>. Afterwards, the
        /// <see cref="Tracker{T}"/> of the  <see cref="MultiTask{T}"/> triggers. This order will happen no matter the
        /// the scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestDependency()
        {
            // Arrange
            const string i = "i";
            
            var expected = new[] { i, i, i, i, i };
            string[] actual = null;

            Task task1 = new ExampleTask();
            MultiTask<string> task2 = new ExampleTask(i);

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(5, tracker1, 2);

            tracker2.result += result => actual = result;
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can be dependent upon another task. This test will check if the
        /// completion of one task can trigger the activation of the <see cref="MultiTask{T}"/>.
        /// </summary>
        /// <expected>The first task will trigger the start of the <see cref="MultiTask{T}"/>. Afterwards, the
        /// <see cref="Tracker{T}"/> of the  <see cref="MultiTask{T}"/> triggers. This order will happen no matter the
        /// the scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestRandomDependency()
        {
            // Arrange
            var amount = RANDOM.Next(2, 10);
            const string check = "i";

            var expected = new string[amount];
            for (var i = 0; i < amount; i++) expected[i] = check;
            
            string[] actual = null;
            
            Task task1 = new ExampleTask();
            MultiTask<string> task2 = new ExampleTask(check);

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(expected, tracker1, 2);
            tracker2.result += result => actual = result;
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can be ended in a <see cref="Tracker{T}.exception"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="MultiTask{T}"/> will trigger the
        /// <see cref="Tracker{T}.exception"/> event when the <see cref="MultiTask{T}"/> throws any one exception.
        /// </expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestException([Values(0, 1000)] int timeout)
        {
            // Arrange
            var e = new Exception();

            List<Exception> expected = new() { e, e, e, e, e };
            List<Exception> actual = null;

            MultiTask<string> task = new ExceptionTask(e);

            // Act
            var tracker = task.Schedule(5, 1, timeout);
            tracker.exception += exception => actual = exception;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask{T}"/> can timeout.
        /// </summary>
        /// <expected>The <see cref="Tracker{T}"/> of the <see cref="MultiTask{T}"/>will trigger the
        /// <see cref="Tracker{T}.exception"/> event when the <see cref="MultiTask{T}"/> has timed out after the
        /// allotted time.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTimeout()
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