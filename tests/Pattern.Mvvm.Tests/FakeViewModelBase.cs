using System;

namespace Pattern.Mvvm.Tests
{
    public class FakeViewModelBase : ViewModelBase
    {
        private string toto;

        public string Toto
        {
            get { return toto; }
            set
            {
                toto = value;
                base.RaisePropertyChanged();
            }
        }

        internal void RaiseProperty(string propertyName = "")
        {
            base.RaisePropertyChanged(propertyName);
        }
    }
}