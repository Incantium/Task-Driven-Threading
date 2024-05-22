using System.Threading;
using NUnit.Framework;
using Obscurum.TDT.Tests.Examples;

namespace Obscurum.TDT.Tests.Scenarios
{
    public class PercentageTest
    {
        [Test, Repeat(10)]
        public void TestPercentage()
        {
           // Arrange
           MultiTask task = new LagTask(100);

           // Act
           var tracker = task.Schedule(2);

           var begin = tracker.percentage;
           
           Thread.Sleep(150);

           var middle = tracker.percentage;
           
           tracker.Wait(1000);

           var end = tracker.percentage;

           // Assert
           Assert.AreEqual(0f, begin);
           Assert.AreEqual(50f, middle);
           Assert.AreEqual(100f, end);
        }
    }
}