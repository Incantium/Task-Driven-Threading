namespace Obscurum.TDT.Tests.Examples
{
    /// <summary>
    /// Class that represents an implementation of the <see cref="Task"/>, <see cref="Task{T}"/>,
    /// <see cref="MultiTask"/> and <see cref="MultiTask{T}"/> interfaces for testing purposes. This class will return
    /// the specified <see cref="message"/> if need be upon completion.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal sealed class ExampleTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly string message;

        public ExampleTask() => message = "";

        public ExampleTask(string message) => this.message = message;
        
        void Task.Execute() {}

        string Task<string>.Execute() => message;

        void MultiTask.Execute(int i) {}

        string MultiTask<string>.Execute(int i) => message;
    }
}