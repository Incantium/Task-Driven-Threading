#pragma warning disable CS0618 // Type or member is obsolete

using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Incantium.TDT.Tests.Scenarios
{
    /// <summary>
    /// Test class to demonstrate many of the features in this package. This class shows:
    /// <ul>
    ///     <li>Multiple types of task interfaces (<see cref="Task"/>, <see cref="Task{T}"/>, <see cref="MultiTask"/>)
    ///     working together.
    ///     </li>
    ///     <li>Task dependency.</li>
    ///     <li>Dynamically calculated <see cref="MultiTask"/>.</li>
    ///     <li>A <see cref="Task{T}"/> with a return type.</li>
    ///     <li>Safe <see cref="Tracker.Wait(int)"/> with automatically timeout.</li>
    /// </ul>
    /// </summary>
    internal sealed class DemonstrationTest
    {
        /// <summary>
        /// Class to generate a <see cref="List{T}"/> of integers by the <see cref="amount"/>, starting at 1.
        /// </summary>
        private class Generator : Task
        {
            private readonly List<int> numbers;
            private readonly int amount;

            public Generator(List<int> numbers, int amount)
            {
                this.numbers = numbers;
                this.amount = amount;
            }

            public void Execute()
            {
                for (var i = 1; i <= amount; i++)
                {
                    numbers.Add(i);
                }
            }
        }

        /// <summary>
        /// Class to convert all integers in a <see cref="List{T}"/> to its power equivalent. This
        /// <see cref="MultiTask"/> takes some time.
        /// </summary>
        private class Power : MultiTask
        {
            private readonly List<int> numbers;

            public Power(List<int> numbers) => this.numbers = numbers;
            
            public void Execute(int i)
            {
                Thread.Sleep(500);
                numbers[i] *= numbers[i];
            }
        }

        /// <summary>
        /// Class to take the sum of a <see cref="List{T}"/> of integers.
        /// </summary>
        private class Sum : Task<int>
        {
            private readonly List<int> numbers;

            public Sum(List<int> numbers) => this.numbers = numbers;
            
            public int Execute()
            {
                var result = 0;
                
                numbers.ForEach(i => result += i);

                return result;
            }
        }

        /// <summary>
        /// Test case to use multiple different tasks in combination.
        /// </summary>
        /// <remarks>The maximum time of this test is 1 second. One single <see cref="Power"/> execution takes 500
        /// milliseconds. Due to <see cref="Power"/> being a <see cref="MultiTask"/> is it still able to be
        /// completed before timeout.</remarks>
        /// <method>This test will use the <see cref="Generator"/> to create a range of integers from 1 to 5. The
        /// <see cref="Power"/> class will then take the power of each integer. The <see cref="Sum"/> class will as last
        /// take the sum of all the power integers, giving a <see cref="Tracker{T}.result"/>.</method>
        /// <expected>The mathematical formula described in this test will have an integer result of 55.</expected>
        /// <since>0.1.0</since>
        [Test, Repeat(10)]
        public void TestDemonstration()
        {
            // Arrange
            const int amount = 5;
            
            const int expected = 55;
            var actual = 0;

            var numbers = new List<int>();

            var task1 = new Generator(numbers, amount);
            var task2 = new Power(numbers);
            var task3 = new Sum(numbers);

            // Act
            var tracker1 = task1.Schedule();
            var tracker2 = task2.Schedule(numbers, tracker1);
            var tracker3 = task3.Schedule(tracker2);

            tracker3.result += result => actual = result;
            
            tracker3.Wait(1000);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}