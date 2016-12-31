using System;

namespace Pattern.Mvvm.Tests
{
    public class FakeBaseViewModel : BaseViewModel
    {
        private string toto;

        public string Toto
        {
            get { return toto; }
            set
            {
                toto = value;
                base.RaiseProperty();
            }
        }

        internal void RaiseProperty(string propertyName = "")
        {
            base.RaiseProperty(propertyName);
        }
    }
}