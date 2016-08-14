namespace Pattern.Core.Tests
{
    using System.Collections.Generic;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Tests.Fakes;

    using Xunit;

    public class InstanciateCollectionObject
    {
        private IKernel kernel;

        public InstanciateCollectionObject()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_a_collection_When_bind_two_class_on_same_interface))]
        public void Should_instanciate_a_collection_When_bind_two_class_on_same_interface()
        {
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass>());
            this.kernel.Bind(typeof(ISimpleClass), this.GetTypeFactory<SimpleClass2>());

            var collection = this.kernel.Get<List<ISimpleClass>>();

            Assert.Equal(2, collection.Count);
            Assert.IsType<SimpleClass>(collection[0]);
            Assert.IsType<SimpleClass2>(collection[1]);
        }
        // Should throw error when bind two_class_on_same_interface

        private TypeFactory GetTypeFactory<T>()
        {
            return new TypeFactory(typeof(T), this.kernel);
        }
    }

    public class SimpleClass2 : ISimpleClass
    {
    }
}