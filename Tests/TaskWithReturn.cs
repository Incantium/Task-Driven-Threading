using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class TaskWithReturn
    {
        [Test, Repeat(10)]
        public void TestTaskWithReturn()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";
            
            Task<string> task = new TaskMessage("Hello World!");
            
            // Act
            var tracker = task.Schedule();
            
            tracker.result += result => actual = result;
            tracker.Wait();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, Repeat(10)]
        public void TestTaskWithDependency()
        {
            // Arrange
            const string expected = "Hello World!";
            var actual = "";
            
            Task<string> task1 = new TaskMessage("Hello ");
            Task<string> task2 = new TaskMessage("World!");
            
            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(tracker1);
        
            tracker1.result += result => actual += result;
            tracker2.result += result => actual += result;
            
            tracker2.Wait(500);
        
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
    
    internal class TaskMessage : Task<string>
    {
        private readonly string message;

        public TaskMessage(string message) => this.message = message;
        
        public string Execute() => message;
    }
}