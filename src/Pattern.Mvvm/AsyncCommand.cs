using System;
using System.Threading.Tasks;
using Pattern.Tasks;

namespace Pattern.Mvvm
{
    public class AsyncCommand<T> : RelayCommand<T>
    {
        private readonly Func<T, Task> method;

        public AsyncCommand(
            Func<T, Task> p,
            ILoadingHandler loadingHandler = null,
                Func<T, bool> canExecute = null)
            : base(t => p(t).Fire(loadingHandler), canExecute)
        {
            this.method = p;
        }

        public Task GeTask(T parameter)
        {
            return this.method(parameter);
        }
    }

    public class AsyncCommand : RelayCommand
    {
        private readonly Func<Task> method;

        public AsyncCommand(
            Func<Task> p,
            ILoadingHandler loadingHandler = null,
            Func<bool> canExecute = null)
            : base(() => p().Fire(loadingHandler), canExecute)
        {
            this.method = p;
        }

        public Task GeTask()
        {
            return this.method();
        }
    }
}