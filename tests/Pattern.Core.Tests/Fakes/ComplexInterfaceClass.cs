namespace Pattern.Core.Tests.Fakes
{
    public class ComplexInterfaceClass
    {
        public ComplexInterfaceClass(ISimpleClass injectedType)
        {
            this.InjectedType = injectedType;
        }

        public ISimpleClass InjectedType { get; set; }
    }
}