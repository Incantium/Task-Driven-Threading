using System;

namespace Obscurum.TDT.Tests.Examples
{
    /// <summary>
    /// Class that represents an implementation of the <see cref="Task"/>, <see cref="Task{T}"/>,
    /// <see cref="MultiTask"/> and <see cref="MultiTask{T}"/> interfaces for testing purposes. This class will throw
    /// the specified <see cref="exception"/> upon being executed.
    /// </summary>
    /// <author>Vanaest</author>
    /// <version>0.1.0</version>
    internal sealed class ExceptionTask : Task, Task<string>, MultiTask, MultiTask<string>
    {
        private readonly Exception exception;

        public ExceptionTask(Exception exception) => this.exception = exception;
        
        void Task.Execute() => throw exception;

        string Task<string>.Execute() => throw exception;

        void MultiTask.Execute(int i) => throw exception;

        string MultiTask<string>.Execute(int i) => throw exception;
    }
}