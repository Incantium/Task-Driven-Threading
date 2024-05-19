using NUnit.Framework;

namespace Obscurum.TDT.Tests
{
    internal class TaskWithoutReturn : Task
    {
        protected override void Execute() {}

        [Test, Repeat(10)]
        public void TestTaskWithReturn()
        {
            // Arrange
            var actual = false;
            
            var task = new TaskWithoutReturn();
            
            // Act
            var tracker = task.Schedule();
            
            tracker.success += () => actual = true;
            tracker.Wait();
            
            // Assert
            Assert.True(actual);
        }
        
        [Test, Repeat(10)]
        public void TestTaskWithDependency()
        {
            // Arrange
            var actual = false;
            
            var task1 = new TaskWithoutReturn();
            var task2 = new TaskWithoutReturn();
            
            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(tracker1);
            
            tracker2.success += () => actual = true;
            tracker2.Wait(500);

            //Assert
            Assert.True(actual);
        }
    }
}