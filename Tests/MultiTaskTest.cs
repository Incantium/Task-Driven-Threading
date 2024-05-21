using System;
using System.Collections.Generic;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    public class MultiTaskTest
    {
        private static readonly Random RANDOM = new();
        
        [Test]
        public void TestMultiTask()
        {
            // Arrange
            var complete = false;

            MultiTask task = new ExampleTask("");

            // Act
            var tracker = task.Schedule(10, 2);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }

        [Test]
        public void TestRandomMultiTask()
        {
            // Arrange
            var complete = false;
            var collection = new int[RANDOM.Next(2, 10)];
            MultiTask task = new ExampleTask("");

            // Act
            var tracker = task.Schedule(collection, 2);
            tracker.success += () => complete = true;
            
            tracker.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }
        
        [Test]
        public void TestDependency()
        {
            // Arrange
            var complete = false;

            Task task1 = new ExampleTask("");
            MultiTask task2 = new ExampleTask("");

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(10, tracker1, 2);
            
            tracker2.success += () => complete = true;
            
            tracker2.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }
        
        [Test]
        public void TestRandomDependency()
        {
            // Arrange
            var complete = false;
            var collection = new int[RANDOM.Next(2, 10)];
            
            Task task1 = new ExampleTask("");
            MultiTask task2 = new ExampleTask("");

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(collection, tracker1, 2);
            tracker2.success += () => complete = true;
            
            tracker2.Wait(1000);

            // Assert
            Assert.IsTrue(complete);
        }
        
        [Test]
        public void TestException()
        {
            // Arrange
            var e = new Exception();

            List<Exception> expected = new() { e, e, e, e, e };
            List<Exception> actual = null;

            MultiTask task = new ExceptionTask(e);

            // Act
            var tracker = task.Schedule(5);
            tracker.exception += exception => actual = exception;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}