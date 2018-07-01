using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Tests.Fakes;

    using Xunit;
    using Pattern.Tests.Xunit;

    public class InstanciateCollectionObject
    {
        private readonly IKernel kernel;

        public InstanciateCollectionObject()
        {
            this.kernel = new Kernel();
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<ElectricMotor>());
            this.kernel.Bind(typeof(IMotor), this.GetTypeFactory<GazoilMotor>());
            this.kernel.Bind(typeof(IGenericMotor<IMotor>), this.GetTypeFactory<ElectricMotor>());
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_bind_two_class_on_same_interface))]
        public void Should_instanciate_a_collection_When_bind_two_class_on_same_interface()
        {
            var collection = this.kernel.Get<List<IMotor>>();

            Assert.Equal(2, collection.Count);
            Assert.IsType<ElectricMotor>(collection[0]);
            Assert.IsType<GazoilMotor>(collection[1]);
        }

        [NamedFact(nameof(Should_instanciate_an_empty_collection_When_no_bind_found_and_get_ienumerable))]
        public void Should_instanciate_an_empty_collection_When_no_bind_found_and_get_ienumerable()
        {
            var collection = this.kernel.Get<IEnumerable<ElectricMotor>>();

            Assert.Empty(collection);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_get_interface_collection))]
        public void Should_instanciate_a_collection_When_get_interface_collection()
        {
            var collection = this.kernel.Get<IList<IMotor>>();

            Assert.Equal(2, collection.Count);
            Assert.IsType<ElectricMotor>(collection[0]);
            Assert.IsType<GazoilMotor>(collection[1]);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_of_generic_interface_When_get_interface_collection))]
        public void Should_instanciate_a_collection_of_generic_interface_When_get_interface_collection()
        {
            var collection = this.kernel.Get<IList<IGenericMotor<IMotor>>>();

            Assert.Equal(1, collection.Count);
            Assert.IsType<ElectricMotor>(collection[0]);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected))]
        public void Should_instanciate_a_collection_When_injected()
        {
            this.kernel.Bind(typeof(MotorsChoice), this.GetTypeFactory<MotorsChoice>());

            var instance = this.kernel.Get<MotorsChoice>();

            Assert.NotNull(instance.AllMotors);
            Assert.Equal(2, instance.AllMotors.Count);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected_in_an_enumerable))]
        public void Should_instanciate_a_collection_When_injected_in_an_enumerable()
        {
            this.kernel.Bind(typeof(EnumerableMotorChoice), this.GetTypeFactory<EnumerableMotorChoice>());

            var instance = this.kernel.Get<EnumerableMotorChoice>();

            Assert.NotNull(instance.AllMotors);
            Assert.Equal(2, instance.AllMotors.Count());
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected_an_enumerable_and_interface_not_implemented))]
        public void Should_instanciate_a_collection_When_injected_an_enumerable_and_interface_not_implemented()
        {
            this.kernel.Bind(typeof(ComplexEnumerableClassWithNotImplementedInterface), this.GetTypeFactory<ComplexEnumerableClassWithNotImplementedInterface>());

            var instance = this.kernel.Get<ComplexEnumerableClassWithNotImplementedInterface>();

            Assert.NotNull(instance.SimpleClasses);
            Assert.Empty(instance.SimpleClasses);
        }

        [NamedFact(nameof(Should_throw_error_When_bind_two_class_same_interface))]
        public void Should_throw_error_When_bind_two_class_same_interface()
        {
            var exception = Assert.Throws<FactoryException>(
                () =>
                {
                    this.kernel.Get(typeof(IMotor));
                });

            Assert.Equal("Injection have found many factories for IMotor.", exception.Message);
        }

        private TypeFactory GetTypeFactory<T>()
        {
            return new TypeFactory(typeof(T), this.kernel);
        }
    }
}