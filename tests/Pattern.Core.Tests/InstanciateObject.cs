namespace Pattern.Core.Tests
{
    using Interfaces;
    using Pattern.Core;

    using Xunit;

    public class InstanciateObject
    {
        private IKernel kernel;

        public InstanciateObject()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_bind_self_type()
        {
            kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_inject_another_type))]
        public void Should_instanciate_type_When_inject_another_type()
        {
            this.kernel.Bind(typeof(ComplexClass), typeof(ComplexClass));
            this.kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = this.kernel.Get(typeof(ComplexClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_map_to_interface))]
        public void Should_instanciate_type_When_map_to_interface()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), typeof(ComplexInterfaceClass));
            this.kernel.Bind(typeof(ISimpleClass), typeof(SimpleClass));

            var instance = this.kernel.Get(typeof(ComplexInterfaceClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexInterfaceClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }
    }

    public interface ISimpleClass
    {
    }

    public class ComplexInterfaceClass
    {
        public ComplexInterfaceClass(ISimpleClass injectedType)
        {
            this.InjectedType = injectedType;
        }

        public ISimpleClass InjectedType { get; set; }
    }
}
