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
        private IKernel kernel;

        public InstanciateCollectionObject()
        {
            this.kernel = new Kernel();
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass2>());
            this.kernel.Bind(typeof(IGenericClass<ISimpleClass>), this.GetTypeFactory<SimpleClass>());
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_bind_two_class_on_same_interface))]
        public void Should_instanciate_a_collection_When_bind_two_class_on_same_interface()
        {
            var collection = this.kernel.Get<List<ISimpleClass>>();

            Assert.Equal(2, collection.Count);
            Assert.IsType<SimpleClass>(collection[0]);
            Assert.IsType<SimpleClass2>(collection[1]);
        }

        [NamedFact(nameof(Should_instanciate_an_empty_collection_When_no_bind_found_and_get_ienumerable))]
        public void Should_instanciate_an_empty_collection_When_no_bind_found_and_get_ienumerable()
        {
            var collection = this.kernel.Get<IEnumerable<SimpleClass>>();

            Assert.Equal(0, collection.Count());
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_get_interface_collection))]
        public void Should_instanciate_a_collection_When_get_interface_collection()
        {
            var collection = this.kernel.Get<IList<ISimpleClass>>();

            Assert.Equal(2, collection.Count);
            Assert.IsType<SimpleClass>(collection[0]);
            Assert.IsType<SimpleClass2>(collection[1]);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_of_generic_interface_When_get_interface_collection))]
        public void Should_instanciate_a_collection_of_generic_interface_When_get_interface_collection()
        {
            var collection = this.kernel.Get<IList<IGenericClass<ISimpleClass>>>();

            Assert.Equal(1, collection.Count);
            Assert.IsType<SimpleClass>(collection[0]);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected))]
        public void Should_instanciate_a_collection_When_injected()
        {
            this.kernel.Bind(typeof(ComplexListClass), this.GetTypeFactory<ComplexListClass>());

            var instance = this.kernel.Get<ComplexListClass>();

            Assert.NotNull(instance.SimpleClasses);
            Assert.Equal(2, instance.SimpleClasses.Count);
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected_in_an_enumerable))]
        public void Should_instanciate_a_collection_When_injected_in_an_enumerable()
        {
            this.kernel.Bind(typeof(ComplexEnumerableClass), this.GetTypeFactory<ComplexEnumerableClass>());

            var instance = this.kernel.Get<ComplexEnumerableClass>();

            Assert.NotNull(instance.SimpleClasses);
            Assert.Equal(2, instance.SimpleClasses.Count());
        }

        [NamedFact(nameof(Should_instanciate_a_collection_When_injected_an_enumerable_and_interface_not_implemented))]
        public void Should_instanciate_a_collection_When_injected_an_enumerable_and_interface_not_implemented()
        {
            this.kernel.Bind(typeof(ComplexEnumerableClassWithNotImplementedInterface), this.GetTypeFactory<ComplexEnumerableClassWithNotImplementedInterface>());

            var instance = this.kernel.Get<ComplexEnumerableClassWithNotImplementedInterface>();

            Assert.NotNull(instance.SimpleClasses);
            Assert.Equal(0, instance.SimpleClasses.Count());
        }

        [NamedFact(nameof(Should_throw_error_When_bind_two_class_same_interface))]
        public void Should_throw_error_When_bind_two_class_same_interface()
        {
            var exception = Assert.Throws<FactoryException>(
                () =>
                {
                    this.kernel.Get(typeof(ISimpleClass));
                });

            Assert.Equal("Injection have found many factories for ISimpleClass.", exception.Message);
        }

        private TypeFactory GetTypeFactory<T>()
        {
            return new TypeFactory(typeof(T), this.kernel);
        }
    }

    public interface IGenericClass<T>
    {
    }
}