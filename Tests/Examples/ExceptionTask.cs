using System;

namespace Obscurum.TDT.Tests.Examples
{
    internal sealed class ExceptionTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly Exception e;

        public ExceptionTask(Exception e) => this.e = e;
        
        void Task.Execute() => throw e;

        string Task<string>.Execute() => throw e;

        void MultiTask.Execute(int i) => throw e;

        string MultiTask<string>.Execute(int i) => throw e;
    }
}