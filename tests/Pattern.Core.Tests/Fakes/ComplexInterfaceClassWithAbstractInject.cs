namespace Pattern.Core.Tests.Fakes
{

    public class ComplexInterfaceClassWithAbstractInject
    {
        public ComplexInterfaceClassWithAbstractInject(AbstractSimpleClass injectedType)
        {
            this.InjectedType = injectedType;
        }

        public AbstractSimpleClass InjectedType { get; set; }
    }
}