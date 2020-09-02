using Xunit;

namespace Pattern.Config.Tests
{
    using NSubstitute;

    using Pattern.Config.Tests.Fakes;
    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Tests.Xunit;

    public class MakeExtensionForBinding
    {
        private readonly IKernel kernel;

        public MakeExtensionForBinding()
        {
            this.kernel = Substitute.For<IKernel>();
        }

        private bool IsTypeFactoryToCreate<T>(IFactory factory)
        {
            var typeFactory = factory as TypeFactory;

            if (typeFactory != null)
            {
                return typeFactory.TypeToCreate == typeof(T);
            }

            return false;
        }

        [NamedFact(nameof(Should_bind_class_to_self_When_use_bind_to_self))]
        public void Should_bind_class_to_self_When_use_bind_to_self()
        {
            this.kernel.Bind<SimpleClass>().ToSelf();

            this.kernel.Received(1)
                .Bind(typeof(SimpleClass), Arg.Is<ComponentFactory>(f => IsTypeFactoryToCreate<SimpleClass>(f.Factory)));
        }

        [NamedFact(nameof(Should_bind_class_When_bind_interface_to_class))]
        public void Should_bind_class_When_bind_interface_to_class()
        {
            this.kernel.Bind<ISimpleClass>().To<SimpleClass>();

            this.kernel.Received(1)
                .Bind(typeof(ISimpleClass), Arg.Is<ComponentFactory>(f => IsTypeFactoryToCreate<SimpleClass>(f.Factory)));
        }

        [NamedFact(nameof(Should_scope_to_singleton_class_When_bind_interface_to_class))]
        public void Should_scope_to_singleton_class_When_bind_interface_to_class()
        {
            this.kernel.Bind<ISimpleClass>().To<SimpleClass>().InSingletonScope();

            this.kernel.Received(1)
                .Bind(typeof(ISimpleClass), Arg.Is<ComponentFactory>(f => f.Factory.GetType() == typeof(SingletonFactory)));
        }

        [NamedFact(nameof(Should_scope_to_singleton_class_When_bind_interface_to_method))]
        public void Should_scope_to_singleton_class_When_bind_interface_to_method()
        {
            this.kernel.Bind<ISimpleClass>().ToMethod(() => new SimpleClass()).InSingletonScope();

            this.kernel.Received(1)
                .Bind(typeof(ISimpleClass), Arg.Is<ComponentFactory>(f => f.Factory.GetType() == typeof(SingletonFactory)));
        }

        [NamedFact(nameof(Should_bind_class_with_a_lambda_factory_When_bind_to_method))]
        public void Should_bind_class_with_a_lambda_factory_When_bind_to_method()
        {
            this.kernel.Bind<ISimpleClass>().ToMethod(() => new SimpleClass());

            this.kernel.Received(1).Bind(typeof(ISimpleClass), Arg.Is<ComponentFactory>(c => c.Factory.GetType() == typeof(LambdaFactory)));
        }

        [NamedFact(nameof(Should_bind_class_When_bind_to_factory))]
        public void Should_bind_class_When_bind_to_factory()
        {
            this.kernel.Get(null, typeof(FakeFactory)).Returns(new FakeFactory());
            
            this.kernel.Bind<ISimpleClass>().ToFactory<FakeFactory>();

            this.kernel.Received(1).Bind(typeof(ISimpleClass), Arg.Any<FakeFactory>());
        }
        
        [NamedFact(nameof(Should_throw_not_found_exception_When_bind_to_factory_not_exists))]
        public void Should_throw_not_found_exception_When_bind_to_factory_not_exists()
        {
            var exception = Assert.Throws<FactoryException>(() =>
                this.kernel.Bind<ISimpleClass>().ToFactory<FakeFactory>());

            Assert.Equal("Injection have not found any factory.", exception.Message);
        }
    }
}