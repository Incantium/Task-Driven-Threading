using System;
using NUnit.Framework;
using Obscurum.TDT.Tasks;

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

            var task = new TaskWithException();

            // Act
            var tracker = task.Schedule();

            tracker.exception += list => actual = list[0];
            tracker.Wait();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}