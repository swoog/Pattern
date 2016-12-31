using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pattern.Mvvm
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}