using System.Threading;

namespace Obscurum.TDT.Tests.Examples
{
    public class LagTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly int milliseconds;

        public LagTask(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }
        
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
            Thread.Sleep(milliseconds);
        }

        string MultiTask<string>.Execute(int i)
        {
            Thread.Sleep(milliseconds);
            return "Complete";
        }
    }
}