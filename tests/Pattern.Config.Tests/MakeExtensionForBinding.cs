namespace Pattern.Config.Tests
{
    using NSubstitute;
    using Pattern.Core.Interfaces;
    using Pattern.Config;
    using Pattern.Config.Tests.Fakes;

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

            this.kernel.Received(1).Bind(typeof(SimpleClass), typeof(SimpleClass));
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_When_bind_interface_to_class))]
        public void Should_bind_class_When_bind_interface_to_class()
        {
            this.kernel.Bind<ISimpleClass>().To<SimpleClass>();

            this.kernel.Received(1).Bind(typeof(ISimpleClass), typeof(SimpleClass));
        }

        [CustomFact(DisplayName = nameof(Should_bind_class_with_a_lambda_factory_When_bind_to_method))]
        public void Should_bind_class_with_a_lambda_factory_When_bind_to_method()
        {
            this.kernel.Bind<ISimpleClass>().ToMethod(() => new SimpleClass());

            this.kernel.Received(1).Bind(typeof(ISimpleClass), Arg.Any<LambdaFactory>());
        }
    }
}
