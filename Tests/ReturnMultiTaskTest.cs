using System;
using System.Collections.Generic;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    public class ReturnMultiTaskTest
    {
        private static readonly Random RANDOM = new();
        
        [Test]
        public void TestMultiTask()
        {
            // Arrange
            var expected = new[] { "i", "i", "i", "i", "i" };
            string[] actual = null;

            MultiTask<string> task = new ExampleTask("i");

            // Act
            var tracker = task.Schedule(5, 2);
            tracker.result += result => actual = result;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRandomMultiTask()
        {
            // Arrange
            var amount = RANDOM.Next(2, 10);
            const string check = "i";

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
        
        [Test]
        public void TestDependency()
        {
            // Arrange
            var expected = new[] { "i", "i", "i", "i", "i" };
            string[] actual = null;

            Task task1 = new ExampleTask("");
            MultiTask<string> task2 = new ExampleTask("i");

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(5, tracker1, 2);

            tracker2.result += result => actual = result;
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void TestRandomDependency()
        {
            // Arrange
            var amount = RANDOM.Next(2, 10);
            const string check = "i";

            var expected = new string[amount];
            for (var i = 0; i < amount; i++) expected[i] = check;
            
            string[] actual = null;
            
            Task task1 = new ExampleTask("");
            MultiTask<string> task2 = new ExampleTask(check);

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(expected, tracker1, 2);
            tracker2.result += result => actual = result;
            
            tracker2.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void TestException()
        {
            // Arrange
            var e = new Exception();

            List<Exception> expected = new() { e, e, e, e, e };
            List<Exception> actual = null;

            MultiTask<string> task = new ExceptionTask(e);

            // Act
            var tracker = task.Schedule(5);
            tracker.exception += exception => actual = exception;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}