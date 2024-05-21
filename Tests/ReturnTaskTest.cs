using System;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests
{
    public class ReturnTaskTest
    {
        [Test]
        public void TestTask()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";

            Task<string> task = new ExampleTask(expected);

            // Act
            var tracker = task.Schedule();
            tracker.result += result => actual = result;
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
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
        
        [Test]
        public void TestException()
        {
            // Arrange
            var expected = new Exception();
            Exception actual = null;

            Task<string> task = new ExceptionTask(expected);

            // Act
            var tracker = task.Schedule();
            tracker.exception += exception => actual = exception[0];
            
            tracker.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}