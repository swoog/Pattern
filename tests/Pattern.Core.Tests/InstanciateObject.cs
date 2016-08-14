namespace Pattern.Core.Tests
{
    using Pattern.Core;
    using Pattern.Core.Interfaces;

    using Xunit;

    public class InstanciateObject
    {
        private readonly IKernel kernel;

        public InstanciateObject()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_get_the_instance_of_kernel_When_get_IKernel))]
        public void Should_get_the_instance_of_kernel_When_get_IKernel()
        {
            var instance = this.kernel.Get(typeof(IKernel));

            Assert.NotNull(instance);
            Assert.Same(this.kernel, instance);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_bind_self_type()
        {
            kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type__When_get_with_generic_method))]
        public void Should_instanciate_type__When_get_with_generic_method()
        {
            this.kernel.Bind(typeof(ISimpleClass), typeof(SimpleClass));

            ISimpleClass instance = this.kernel.Get<ISimpleClass>();

            Assert.NotNull(instance);
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

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_use_custom_factory))]
        public void Should_instanciate_type_When_use_custom_factory()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), typeof(ComplexInterfaceClass));
            this.kernel.Bind(typeof(ISimpleClass), new LambdaFactory(() => new SimpleClass()));

            var instance = this.kernel.Get(typeof(ComplexInterfaceClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexInterfaceClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }

        [CustomFact(DisplayName = nameof(Should_display_an_error_message_When_instanciate_an_unknow_object))]
        public void Should_display_an_error_message_When_instanciate_an_unknow_object()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), typeof(ComplexInterfaceClass));

            var exception = Assert.Throws<InjectionException>(
                () =>
                    {
                        this.kernel.Get(typeof(ComplexInterfaceClass));
                    });

            Assert.Equal("Injection not found for ISimpleClass when injected in ComplexInterfaceClass.", exception.Message);
        }
    }
}
