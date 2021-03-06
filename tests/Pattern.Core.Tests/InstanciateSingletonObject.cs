using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests
{
    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Tests.Fakes;
    using Pattern.Tests.Xunit;

    using Xunit;

    public class InstanciateSingletonObject
    {
        private readonly IKernel kernel;

        public InstanciateSingletonObject()
        {
            this.kernel = new Kernel();
        }

        [NamedFact(nameof(Should_get_the_same_instance_when_set_singleton))]
        public void Should_get_the_same_instance_when_set_singleton()
        {
            var typeFactory = new TypeFactory(typeof(ElectricMotor), this.kernel);
            var singletonFactory = new SingletonFactory(typeFactory);
            this.kernel.Bind(typeof(IMotor), singletonFactory);

            var instance1 = this.kernel.Get(typeof(IMotor));
            var instance2 = this.kernel.Get(typeof(IMotor));

            Assert.Same(instance1, instance2);
        }
    }
}