using System;
using System.Windows.Input;

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
}