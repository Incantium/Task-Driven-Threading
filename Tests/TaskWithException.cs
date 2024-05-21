using System;
using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class TaskWithException : Task
    {
        private static readonly Exception expected = new();
        
        public void Execute() => throw expected;

        [Test, Repeat(10)]
        public void TestTaskWithException()
        {
            // Arrange
            Exception actual = null;

            Task task = new TaskWithException();

            // Act
            var tracker = task.Schedule();

            tracker.exception += list => actual = list[0];
            tracker.Wait();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}