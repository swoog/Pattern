namespace Pattern.Config.Tests
{
    using NSubstitute;
    using Pattern.Core.Interfaces;
    using Pattern.Config;
    using Pattern.Config.Tests.Fakes;
    using Pattern.Core.Interfaces.Factories;

    public class MakeExtensionForBinding
    {
        private readonly IKernel kernel;

        public MakeExtensionForBinding()
        {
            this.kernel = Substitute.For<IKernel>();
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_to_self_When_use_bind_to_self))]
        public void Should_bind_class_to_self_When_use_bind_to_self()
        {
            this.kernel.Bind<SimpleClass>().ToSelf();

            this.kernel.Received(1)
                .Bind(typeof(SimpleClass), Arg.Is<TypeFactory>(f => f.TypeToCreate == typeof(SimpleClass)));
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_When_bind_interface_to_class))]
        public void Should_bind_class_When_bind_interface_to_class()
        {
            this.kernel.Bind<ISimpleClass>().To<SimpleClass>();

            this.kernel.Received(1)
                .Bind(typeof(ISimpleClass), Arg.Is<TypeFactory>(f => f.TypeToCreate == typeof(SimpleClass)));
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_with_a_lambda_factory_When_bind_to_method))]
        public void Should_bind_class_with_a_lambda_factory_When_bind_to_method()
        {
            this.kernel.Bind<ISimpleClass>().ToMethod(() => new SimpleClass());

            this.kernel.Received(1).Bind(typeof(ISimpleClass), Arg.Any<LambdaFactory>());
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_When_bind_to_factory))]
        public void Should_bind_class_When_bind_to_factory()
        {
            this.kernel.Bind<ISimpleClass>().ToFactory<FakeFactory>();

            this.kernel.Received(1).Bind(typeof(ISimpleClass), Arg.Any<FakeFactory>());
        }
    }

    public class FakeFactory : IFactory
    {
        public object Create(object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
