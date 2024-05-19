using System;
using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class TaskWithReturnException : Task<int>
    {
        private static readonly Exception expected = new();
        
        protected override int Execute() => throw expected;

        [Test, Repeat(10)]
        public void TestTaskWithException()
        {
            // Arrange
            Exception actual = null;

            var task = new TaskWithReturnException();

            // Act
            var tracker = task.Schedule();

            tracker.exception += list => actual = list[0];
            tracker.Wait();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}