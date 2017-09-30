using Pattern.Core.Tests.Example;

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
            kernel.Bind(typeof(MotorWithTwoConstructor), this.GetTypeFactory<MotorWithTwoConstructor>());

            var instance = kernel.Get(typeof(MotorWithTwoConstructor));

            Assert.NotNull(instance);
            Assert.IsType<MotorWithTwoConstructor>(instance);
        }

        [NamedFact(nameof(Should_choice_biggest_constructor_type_When_have_more_than_one_constructor))]
        public void Should_choice_biggest_constructor_type_When_have_more_than_one_constructor()
        {
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<ElectricMotor>());
            kernel.Bind(typeof(SimpleClassWithConstructors), this.GetTypeFactory<SimpleClassWithConstructors>());

            var instance = kernel.Get(typeof(SimpleClassWithConstructors));

            Assert.NotNull(instance);
            var typedInstance = Assert.IsType<SimpleClassWithConstructors>(instance);
            Assert.NotNull(typedInstance.Motor);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_static_constructor))]
        public void Should_instanciate_type_When_have_static_constructor()
        {
            kernel.Bind(typeof(MotorWithStaticConstructor), this.GetTypeFactory<MotorWithStaticConstructor>());

            var instance = kernel.Get(typeof(MotorWithStaticConstructor));

            Assert.NotNull(instance);
            Assert.IsType<MotorWithStaticConstructor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_have_static_constructor_and_an_injected_constructor))]
        public void Should_instanciate_type_When_have_static_constructor_and_an_injected_constructor()
        {
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<ElectricMotor>());
            this.kernel.Bind(typeof(SimpleClassWithStaticConstructorAndInjectedConstructor), this.GetTypeFactory<SimpleClassWithStaticConstructorAndInjectedConstructor>());

            var instance = kernel.Get(typeof(SimpleClassWithStaticConstructorAndInjectedConstructor));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClassWithStaticConstructorAndInjectedConstructor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_bind_self_type()
        {
            kernel.Bind(typeof(ElectricMotor), this.GetTypeFactory<ElectricMotor>());

            var instance = kernel.Get(typeof(ElectricMotor));

            Assert.NotNull(instance);
            Assert.IsType<ElectricMotor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_use_auto_bind_concret_type()
        {
            var instance = kernel.Get(typeof(ElectricMotor));

            Assert.NotNull(instance);
            Assert.IsType<ElectricMotor>(instance);
        }

        [NamedFact(nameof(Should_instanciate_type__When_get_with_generic_method))]
        public void Should_instanciate_type__When_get_with_generic_method()
        {
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<ElectricMotor>());

            IMotor instance = this.kernel.Get<IMotor>();

            Assert.NotNull(instance);
        }

        [NamedFact(nameof(Should_instanciate_type_When_inject_another_type))]
        public void Should_instanciate_type_When_inject_another_type()
        {
            this.kernel.Bind(typeof(ElectricCarVehicle), this.GetTypeFactory<ElectricCarVehicle>());
            this.kernel.Bind(typeof(ElectricMotor), this.GetTypeFactory<ElectricMotor>());

            var instance = this.kernel.Get(typeof(ElectricCarVehicle));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ElectricCarVehicle>(instance);
            Assert.NotNull(complexType.Motor);
        }

        [NamedFact(nameof(Should_instanciate_type_When_map_to_interface))]
        public void Should_instanciate_type_When_map_to_interface()
        {
            this.kernel.Bind(typeof(CarVehicle), this.GetTypeFactory<CarVehicle>());
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<ElectricMotor>());

            var instance = this.kernel.Get(typeof(CarVehicle));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<CarVehicle>(instance);
            Assert.NotNull(complexType.Motor);
        }

        [NamedFact(nameof(Should_instanciate_type_When_use_custom_factory))]
        public void Should_instanciate_type_When_use_custom_factory()
        {
            this.kernel.Bind(typeof(CarVehicle), this.GetTypeFactory<CarVehicle>());
            this.kernel.Bind(typeof(IMotor), new LambdaFactory(c => new ElectricMotor()));

            var instance = this.kernel.Get(typeof(CarVehicle));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<CarVehicle>(instance);
            Assert.NotNull(complexType.Motor);
        }

        [NamedFact(nameof(Should_display_an_error_message_When_instanciate_an_unknow_object))]
        public void Should_display_an_error_message_When_instanciate_an_unknow_object()
        {
            this.kernel.Bind(typeof(CarVehicle), this.GetTypeFactory<CarVehicle>());

            var exception = Assert.Throws<InjectionException>(
                () =>
                    {
                        this.kernel.Get(typeof(CarVehicle));
                    });

            Assert.Equal("Injection not found for IMotor when injected in CarVehicle.", exception.Message);
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
            var canResolve = this.kernel.CanResolve(null, typeof(IMotor));

            Assert.False(canResolve);
        }

        [NamedFact(nameof(Should_instanciate_generic_type_When_have_a_generic_parameter))]
        public void Should_instanciate_generic_type_When_have_a_generic_parameter()
        {
            kernel.Bind(typeof(IOptions<>), new TypeFactory(typeof(Options<>), this.kernel));

            var instance = kernel.Get(typeof(IOptions<ElectricMotor>));

            Assert.NotNull(instance);
            Assert.IsType<Options<ElectricMotor>>(instance);
        }

        [NamedFact(nameof(Should_instanciate_generic_type_When_have_a_generic_but_no_parameter))]
        public void Should_instanciate_generic_type_When_have_a_generic_but_no_parameter()
        {
            kernel.Bind(typeof(IOptions<ElectricMotor>), new TypeFactory(typeof(Options<ElectricMotor>), this.kernel));

            var instance = kernel.Get(typeof(IOptions<ElectricMotor>));

            Assert.NotNull(instance);
            Assert.IsType<Options<ElectricMotor>>(instance);
        }

        [NamedFact(nameof(Should_return_null_When_have_a_generic_parameter_and_no_binding))]
        public void Should_return_null_When_have_a_generic_parameter_and_no_binding()
        {
            var instance = kernel.Get(typeof(IOptions<ElectricMotor>));

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
        public IMotor Motor { get; }

        public SimpleClassWithConstructors()
        {

        }

        public SimpleClassWithConstructors(IMotor motor)
        {
            this.Motor = motor;
        }
    }
}
