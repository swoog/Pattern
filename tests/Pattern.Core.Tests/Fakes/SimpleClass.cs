namespace Pattern.Core.Tests.Fakes
{
    public class SimpleClass : ISimpleClass
    {
    }

    public class SimpleClassWithStaticConstructor : ISimpleClass
    {
        public static ISimpleClass simpleClass;

        static SimpleClassWithStaticConstructor()
        {
            simpleClass = new SimpleClassWithStaticConstructor();
        }
    }

    public class SimpleClassWithStaticConstructorAndInjectedConstructor
    {
        public static ISimpleClass simpleClassStatic;
        private readonly ISimpleClass simpleClass;

        public SimpleClassWithStaticConstructorAndInjectedConstructor(ISimpleClass simpleClass)
        {
            this.simpleClass = simpleClass;
        }

        static SimpleClassWithStaticConstructorAndInjectedConstructor()
        {
            simpleClassStatic = new SimpleClassWithStaticConstructor();
        }
    }

    public class SimpleClassWithTwoConstructor : ISimpleClass
    {
        public SimpleClassWithTwoConstructor()
        {
        }

        public SimpleClassWithTwoConstructor(string val1)
        {
        }
    }
}