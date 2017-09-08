using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Pattern.Tasks;

namespace Pattern.Mvvm
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> action;

        public RelayCommand(Action<T> action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action((T)parameter);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action action;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
    }

    public class AsyncCommand : RelayCommand
    {
        private readonly Func<Task> method;

        public AsyncCommand(Func<Task> p, ILoadingHandler loadingHandler = null)
            : base(() => p().Fire(loadingHandler))
        {
            this.method = p;
        }

        public Task GeTask()
        {
            return this.method();
        }
    }

    public class AsyncCommand<T> : RelayCommand<T>
    {
        private readonly Func<T, Task> method;

        public AsyncCommand(Func<T, Task> p)
            : base(t => p(t).Fire())
        {
            this.method = p;
        }

        public Task GeTask(T parameter)
        {
            return this.method(parameter);
        }
    }
    public class ViewModelBase : INotifyPropertyChanged
    {
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;

                this.RaisePropertyChanged(propertyName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}