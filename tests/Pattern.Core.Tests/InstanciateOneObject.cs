namespace Pattern.Core.Tests
{
    using Pattern.Core;
    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Tests.Fakes;
    using Pattern.Tests.Xunit;
    using Xunit;

    public class InstanciateOneObject
    {
        private readonly IKernel kernel;

        public InstanciateOneObject()
        {
            this.kernel = new Kernel();
        }

        [NamedFact(nameof(Should_get_the_instance_of_kernel_When_get_IKernel))]
        public void Should_get_the_instance_of_kernel_When_get_IKernel()
        {
            var instance = this.kernel.Get(typeof(IKernel));

            Assert.NotNull(instance);
            Assert.Same(this.kernel, instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_two_constructor))]
        public void Should_instanciate_type_When_have_two_constructor()
        {
            kernel.Bind(typeof(SimpleClassWithTwoConstructor), this.GetTypeFactory<SimpleClassWithTwoConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithTwoConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithTwoConstructor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_static_constructor))]
        public void Should_instanciate_type_When_have_static_constructor()
        {
            kernel.Bind(typeof(SimpleClassWithStaticConstructor), this.GetTypeFactory<SimpleClassWithStaticConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithStaticConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithStaticConstructor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_bind_self_type()
        {
            kernel.Bind(typeof(SimpleClass), this.GetTypeFactory<SimpleClass>());

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_use_auto_bind_concret_type()
        {
            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type__When_get_with_generic_method))]
        public void Should_instanciate_type__When_get_with_generic_method()
        {
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());

            ISimpleClass instance = this.kernel.Get<ISimpleClass>();

            Assert.NotNull(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_inject_another_type))]
        public void Should_instanciate_type_When_inject_another_type()
        {
            this.kernel.Bind(typeof(ComplexClass), this.GetTypeFactory<ComplexClass>());
            this.kernel.Bind(typeof(SimpleClass), this.GetTypeFactory<SimpleClass>());

            var instance = this.kernel.Get(typeof(ComplexClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }

        [NamedFact(nameof(Should_instanciate_type_When_map_to_interface))]
        public void Should_instanciate_type_When_map_to_interface()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), this.GetTypeFactory<ComplexInterfaceClass>());
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());

            var instance = this.kernel.Get(typeof(ComplexInterfaceClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexInterfaceClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }

        [NamedFact(nameof(Should_instanciate_type_When_use_custom_factory))]
        public void Should_instanciate_type_When_use_custom_factory()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), this.GetTypeFactory<ComplexInterfaceClass>());
            this.kernel.Bind(typeof(ISimpleClass), new LambdaFactory(() => new SimpleClass()));

            var instance = this.kernel.Get(typeof(ComplexInterfaceClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexInterfaceClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }

        [NamedFact(nameof(Should_display_an_error_message_When_instanciate_an_unknow_object))]
        public void Should_display_an_error_message_When_instanciate_an_unknow_object()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClass), this.GetTypeFactory<ComplexInterfaceClass>());

            var exception = Assert.Throws<InjectionException>(
                () =>
                    {
                        this.kernel.Get(typeof(ComplexInterfaceClass));
                    });

            Assert.Equal("Injection not found for ISimpleClass when injected in ComplexInterfaceClass.", exception.Message);
        }

        [NamedFact(nameof(Should_display_an_error_message_When_instanciate_an_unknow_object_from_abstract))]
        public void Should_display_an_error_message_When_instanciate_an_unknow_object_from_abstract()
        {
            this.kernel.Bind(typeof(ComplexInterfaceClassWithAbstractInject), this.GetTypeFactory<ComplexInterfaceClassWithAbstractInject>());

            var exception = Assert.Throws<InjectionException>(
                () =>
                {
                    this.kernel.Get(typeof(ComplexInterfaceClassWithAbstractInject));
                });

            Assert.Equal("Injection not found for AbstractSimpleClass when injected in ComplexInterfaceClassWithAbstractInject.", exception.Message);
        }


        private TypeFactory GetTypeFactory<T>()
        {
            return new TypeFactory(typeof(T), this.kernel);
        }
    }
}
