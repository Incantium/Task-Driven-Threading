using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests.Scenarios
{
    public class PercentageTest
    {
        [Test]
        public void TestPercentage()
        {
           // Arrange
           Task task = new LagTask(50);

           // Act
           var tracker = task.Schedule();

           var begin = tracker.percentage;
           
           tracker.Wait(1000);

           var end = tracker.percentage;

           // Assert
           Assert.AreEqual(0f, begin);
           Assert.AreEqual(100f, end);
        }
    }
}