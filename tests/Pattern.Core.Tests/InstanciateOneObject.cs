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

        [NamedFact(nameof(Should_inject_the_instance_of_kernel_When_constructor_have_IKernel))]
        public void Should_inject_the_instance_of_kernel_When_constructor_have_IKernel()
        {
            var instance = this.kernel.Get(typeof(SimpleClassWithKernelConstructor));

            Assert.NotNull(instance);
            var typeInstance = Assert.IsType<SimpleClassWithKernelConstructor>(instance);
            Assert.Same(this.kernel, typeInstance.Kernel);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_two_constructor))]
        public void Should_instanciate_type_When_have_two_constructor()
        {
            kernel.Bind(typeof(SimpleClassWithTwoConstructor), this.GetTypeFactory<SimpleClassWithTwoConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithTwoConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithTwoConstructor>(instance);
        }

        [NamedFact(nameof(Should_choice_biggest_constructor_type_When_have_more_than_one_constructor))]
        public void Should_choice_biggest_constructor_type_When_have_more_than_one_constructor()
        {
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());
            kernel.Bind(typeof(SimpleClassWithConstructors), this.GetTypeFactory<SimpleClassWithConstructors>());

            var instance = kernel.Get(typeof(SimpleClassWithConstructors));

            Assert.NotNull(instance);
            var typedInstance = Assert.IsType<SimpleClassWithConstructors>(instance);
            Assert.NotNull(typedInstance.SimpleClass);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_static_constructor))]
        public void Should_instanciate_type_When_have_static_constructor()
        {
            kernel.Bind(typeof(SimpleClassWithStaticConstructor), this.GetTypeFactory<SimpleClassWithStaticConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithStaticConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithStaticConstructor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_static_constructor_and_an_injected_constructor))]
        public void Should_instanciate_type_When_have_static_constructor_and_an_injected_constructor()
        {
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());
            this.kernel.Bind(typeof(SimpleClassWithStaticConstructorAndInjectedConstructor), this.GetTypeFactory<SimpleClassWithStaticConstructorAndInjectedConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithStaticConstructorAndInjectedConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithStaticConstructorAndInjectedConstructor>(instance);
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
            this.kernel.Bind(typeof(ISimpleClass), new LambdaFactory(c => new SimpleClass()));

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

        [NamedFact(nameof(Should_return_false_When_test_can_resolve_interface))]
        public void Should_return_false_When_test_can_resolve_interface()
        {
            var canResolve = this.kernel.CanResolve(null, typeof(ISimpleClass));

            Assert.False(canResolve);
        }

        [NamedFact(nameof(Should_instanciate_generic_type_When_have_a_generic_parameter))]
        public void Should_instanciate_generic_type_When_have_a_generic_parameter()
        {
            kernel.Bind(typeof(IOptions<>), new TypeFactory(typeof(Options<>), this.kernel));

            var instance = kernel.Get(typeof(IOptions<SimpleClass>));

            Assert.NotNull(instance);
            Assert.IsType<Options<SimpleClass>>(instance);
        }

        [NamedFact(nameof(Should_instanciate_generic_type_When_have_a_generic_but_no_parameter))]
        public void Should_instanciate_generic_type_When_have_a_generic_but_no_parameter()
        {
            kernel.Bind(typeof(IOptions<SimpleClass>), new TypeFactory(typeof(Options<SimpleClass>), this.kernel));

            var instance = kernel.Get(typeof(IOptions<SimpleClass>));

            Assert.NotNull(instance);
            Assert.IsType<Options<SimpleClass>>(instance);
        }

        [NamedFact(nameof(Should_return_null_When_have_a_generic_parameter_and_no_binding))]
        public void Should_return_null_When_have_a_generic_parameter_and_no_binding()
        {
            var instance = kernel.Get(typeof(IOptions<SimpleClass>));

            Assert.Null(instance);
        }

        private TypeFactory GetTypeFactory<T>()
        {
            return new TypeFactory(typeof(T), this.kernel);
        }
    }

    public class Options<T> : IOptions<T>
    {
    }

    public interface IOptions<T>
    {
    }

    public class SimpleClassWithConstructors
    {
        public ISimpleClass SimpleClass { get; }

        public SimpleClassWithConstructors()
        {

        }

        public SimpleClassWithConstructors(ISimpleClass simpleClass)
        {
            this.SimpleClass = simpleClass;
        }
    }
}
