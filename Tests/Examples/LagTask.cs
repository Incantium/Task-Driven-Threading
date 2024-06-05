using System.Threading;

namespace Incantium.TDT.Tests.Examples
{
    /// <summary>
    /// Class that represents an implementation of the <see cref="Task"/>, <see cref="Task{T}"/>,
    /// <see cref="MultiTask"/> and <see cref="MultiTask{T}"/> interfaces for testing purposes. This class will create
    /// (increasingly) artificial lag for the amount of <see cref="milliseconds"/> specified upon execution.
    /// </summary>
    internal sealed class LagTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly int milliseconds;

        public LagTask(int milliseconds) => this.milliseconds = milliseconds;
        
        void Task.Execute()
        {
            Thread.Sleep(milliseconds);
        }

        string Task<string>.Execute()
        {
            Thread.Sleep(milliseconds);
            return "Complete";
        }

        void MultiTask.Execute(int i)
        {
            Thread.Sleep(milliseconds * (i + 1));
        }

        string MultiTask<string>.Execute(int i)
        {
            Thread.Sleep(milliseconds * (i + 1));
            return "Complete";
        }
    }
}