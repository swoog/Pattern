namespace Pattern.Core.Tests
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