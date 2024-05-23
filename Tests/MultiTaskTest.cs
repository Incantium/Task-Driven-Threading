using System;
using System.Collections.Generic;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    /// <summary>
    /// Test class for <see cref="MultiTask"/>.
    /// </summary>
    /// <author>Vanaest</author>
    internal sealed class MultiTaskTest
    {
        private static readonly Random RANDOM = new();
        
        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can be started and completed with a <see cref="Tracker.success"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <param name="batch">Test at different batch sizes.</param>
        /// <expected>The <see cref="Tracker"/> of the <see cref="MultiTask"/> will trigger the
        /// <see cref="Tracker.success"/> event when the all the single tasks of the <see cref="MultiTask"/> are
        /// completed.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestMultiTask([Range(1, 5)] int batch, [Values(0, 1000)] int timeout)
        {
            // Arrange
            var complete = false;

            MultiTask task = new ExampleTask();

            // Act
            var tracker = task.Schedule(5, batch, timeout);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }

        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can be started and completed with a <see cref="Tracker.success"/>
        /// with a random amount at runtime.
        /// </summary>
        /// <expected>The <see cref="Tracker"/> of the <see cref="MultiTask"/> will trigger the
        /// <see cref="Tracker.success"/> event when the all the single tasks of the <see cref="MultiTask"/> are
        /// completed.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestRandomMultiTask()
        {
            // Arrange
            var complete = false;
            var collection = new int[RANDOM.Next(2, 10)];
            MultiTask task = new ExampleTask();

            // Act
            var tracker = task.Schedule(collection, 2);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can be dependent upon another task. This test will check if the
        /// completion of one task can trigger the activation of the <see cref="MultiTask"/>.
        /// </summary>
        /// <expected>The first task will trigger the start of the <see cref="MultiTask"/>. Afterwards, the
        /// <see cref="Tracker"/> of the  <see cref="MultiTask"/> triggers. This order will happen no matter the the
        /// scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestDependency()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";

            Task task1 = new ExampleTask();
            MultiTask task2 = new ExampleTask();

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(10, tracker1, 2);

            tracker1.success += () => actual += "Hello ";
            tracker2.success += () => actual += "World!";
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can be dependent upon another task. This test will check if the
        /// completion of one task can trigger the activation of the <see cref="MultiTask"/>.
        /// </summary>
        /// <expected>The first task will trigger the start of the <see cref="MultiTask"/>. Afterwards, the
        /// <see cref="Tracker"/> of the  <see cref="MultiTask"/> triggers. This order will happen no matter the the
        /// scheduling order of the tasks.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestRandomDependency()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";
            var collection = new int[RANDOM.Next(2, 10)];
            
            Task task1 = new ExampleTask();
            MultiTask task2 = new ExampleTask();

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(collection, tracker1, 2);

            tracker1.success += () => actual += "Hello ";
            tracker2.success += () => actual += "World!";
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can be ended in a <see cref="Tracker.exception"/>.
        /// </summary>
        /// <param name="timeout">Test without and with a maximum timeout in milliseconds.</param>
        /// <expected>The <see cref="Tracker"/> of the <see cref="MultiTask"/> will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="MultiTask"/> throws any one exception.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestException([Values(0, 1000)] int timeout)
        {
            // Arrange
            var e = new Exception();

            List<Exception> expected = new() { e, e, e, e, e };
            List<Exception> actual = null;

            MultiTask task = new ExceptionTask(e);

            // Act
            var tracker = task.Schedule(5, 1, timeout);
            tracker.exception += exception => actual = exception;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Test case if a <see cref="MultiTask"/> can timeout.
        /// </summary>
        /// <expected>The <see cref="Tracker"/> of the <see cref="MultiTask"/>will trigger the
        /// <see cref="Tracker.exception"/> event when the <see cref="MultiTask"/> has timed out after the allotted
        /// time.</expected>
        /// <version>1.0.0</version>
        [Test, Repeat(10)]
        public void TestTimeout()
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
    }
}