namespace Pattern.Core.Tests.Fakes
{
    public class ComplexClass
    {
        public ComplexClass(SimpleClass injectedType)
        {
            this.InjectedType = injectedType;
        }

        public SimpleClass InjectedType { get; set; }
    }
}