namespace Pattern.Core.Tests
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