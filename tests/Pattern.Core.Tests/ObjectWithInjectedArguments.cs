namespace Pattern.Core.Tests
{
    using Pattern.Core.Tests.Fakes;

    public class ObjectWithInjectedArguments : ObjectWithArguments
    {
        public SimpleClass SimpleClass { get; set; }

        public ObjectWithInjectedArguments(int i, SimpleClass simpleClass, string stringValue)
            : base(i, stringValue)
        {
            this.SimpleClass = simpleClass;
        }
    }
}