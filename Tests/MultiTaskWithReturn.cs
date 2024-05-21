using System;
using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class MultiTaskWithReturn : MultiTask<int>
    {
        private static readonly Random RANDOM = new();
        
        public int Execute(int i) => i;

        [Test, Repeat(10)]
        public void TestMultiTaskWithReturn()
        {
            // Arrange
            var expected = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] actual = null;

            MultiTask<int> task = new MultiTaskWithReturn();

            // Act
            var tracker = task.Schedule(10);

            tracker.result += result => actual = result;
            tracker.exception += list => Assert.Fail(list[0].ToString());
            tracker.Wait();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, Repeat(10)]
        public void TestMultiTaskWithRandomReturn()
        {
            // Arrange
            var length = RANDOM.Next(5, 20);
            
            var expected = new int[length];
            int[] actual = null;
            
            for (var i = 0; i < length; i++) expected[i] = i;
            
            MultiTask<int> task = new MultiTaskWithReturn();

            // Act
            var tracker = task.Schedule(expected);

            tracker.result += result => actual = result;
            tracker.Wait();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiTaskWithBatches([Range(1, 10)] int batch)
        {
            // Arrange
            var expected = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] actual = null;

            MultiTask<int> task = new MultiTaskWithReturn();

            // Act
            var tracker = task.Schedule(10, batch);

            tracker.result += result => actual = result;
            tracker.Wait();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}