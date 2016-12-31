using System;
using System.Windows.Input;

namespace Pattern.Mvvm.Tests
{
    internal class RelayCommand : ICommand
    {
        private Action action;
        private Func<bool> canExecute;

        public RelayCommand()
        {
        }

        public RelayCommand(Action action, Func<bool> canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            bool? canExecute = this.canExecute?.Invoke();
            return canExecute ?? true;
        }

        public void Execute(object parameter)
        {
            if(this.CanExecute(parameter))
            {
                this.action?.Invoke();
            }
        }
    }
}