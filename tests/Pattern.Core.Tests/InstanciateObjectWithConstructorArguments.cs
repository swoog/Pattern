namespace Pattern.Core.Tests
{
    using Pattern.Core.Interfaces;
    using Pattern.Core.Tests.Fakes;

    using Xunit;

    public class InstanciateObjectWithConstructorArguments
    {
        private readonly IKernel kernel;

        public InstanciateObjectWithConstructorArguments()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_object_When_have_custom_arguments))]
        public void Should_instanciate_object_When_have_custom_arguments()
        {
            var instance = this.kernel.Get(null, typeof(ObjectWithArguments), 1, "Toto");

            var instanceOfObjsctWithArguments = Assert.IsType<ObjectWithArguments>(instance);
            Assert.Equal("Toto", instanceOfObjsctWithArguments.StringValue);
            Assert.Equal(1, instanceOfObjsctWithArguments.IntValue);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_object_When_have_custom_arguments_with_an_injected_argument))]
        public void Should_instanciate_object_When_have_custom_arguments_with_an_injected_argument()
        {
            var instance = this.kernel.Get(null, typeof(ObjectWithInjectedArguments), 1, "Toto");

            var instanceOfObjsctWithArguments = Assert.IsType<ObjectWithInjectedArguments>(instance);
            Assert.Equal("Toto", instanceOfObjsctWithArguments.StringValue);
            Assert.Equal(1, instanceOfObjsctWithArguments.IntValue);
            Assert.NotNull(instanceOfObjsctWithArguments.SimpleClass);
        }

        [CustomFact(DisplayName = nameof(Should_make_an_injection_error_When_custom_arguments_does_not_corresponding))]
        public void Should_make_an_injection_error_When_custom_arguments_does_not_corresponding()
        {
            var exception = Assert.Throws<InjectionException>(
                () =>
                {
                    this.kernel.Get(null, typeof(ObjectWithInjectedArguments), "Toto");
                });

            Assert.Equal("Injection not found for Int32 when injected in ObjectWithInjectedArguments.", exception.Message);
        }
    }
}