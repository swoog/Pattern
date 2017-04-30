namespace Pattern.Core.Tests
{
    using System;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Tests.Fakes;
    using Pattern.Tests.Xunit;
    using Xunit;

    public class InstanciateObjectWithConstructorArguments
    {
        private readonly IKernel kernel;

        public InstanciateObjectWithConstructorArguments()
        {
            this.kernel = new Kernel();
        }

        [NamedFact(nameof(Should_instanciate_object_When_have_custom_arguments))]
        public void Should_instanciate_object_When_have_custom_arguments()
        {
            var instance = this.kernel.Get(null, typeof(ObjectWithArguments), 1, "Toto");

            var instanceOfObjsctWithArguments = Assert.IsType<ObjectWithArguments>(instance);
            Assert.Equal("Toto", instanceOfObjsctWithArguments.StringValue);
            Assert.Equal(1, instanceOfObjsctWithArguments.IntValue);
        }

        [NamedFact(nameof(Should_instanciate_object_When_have_custom_arguments_with_an_injected_argument))]
        public void Should_instanciate_object_When_have_custom_arguments_with_an_injected_argument()
        {
            var instance = this.kernel.Get(null, typeof(ObjectWithInjectedArguments), 1, "Toto");

            var instanceOfObjsctWithArguments = Assert.IsType<ObjectWithInjectedArguments>(instance);
            Assert.Equal("Toto", instanceOfObjsctWithArguments.StringValue);
            Assert.Equal(1, instanceOfObjsctWithArguments.IntValue);
            Assert.NotNull(instanceOfObjsctWithArguments.SimpleClass);
        }

        [NamedFact(nameof(Should_make_an_injection_error_When_custom_arguments_does_not_corresponding))]
        public void Should_make_an_injection_error_When_custom_arguments_does_not_corresponding()
        {
            var exception = Assert.Throws<ConstructiorSearchException>(
                () =>
                {
                    this.kernel.Get(null, typeof(ObjectWithInjectedArguments), "Toto");
                });

            Assert.Equal("Can create instance of ObjectWithInjectedArguments.", exception.Message);
        }

        [NamedFact(nameof(Should_throw_argument_exception_When_instanciate_type_null))]
        public void Should_throw_argument_exception_When_instanciate_type_null()
        {
            var exception = Assert.Throws<ArgumentException>(
                () =>
                    {
                        this.kernel.Get(null, null);
                    });

            Assert.Equal("From type cannot be null", exception.Message);
        }
    }
}