namespace Obscurum.TDT.Tests.Examples
{
    public class ExampleTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly string result;

        public ExampleTask(string result) => this.result = result;
        
        void Task.Execute() {}

        string Task<string>.Execute() => result;

        void MultiTask.Execute(int i) {}

        string MultiTask<string>.Execute(int i) => result;
    }
}