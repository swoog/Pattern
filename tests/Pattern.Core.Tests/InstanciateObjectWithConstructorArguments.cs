using Pattern.Core.Tests.Example;

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

        [NamedFact(nameof(Should_make_an_injection_error_When_custom_arguments_does_not_corresponding))]
        public void Should_make_an_injection_error_When_custom_arguments_does_not_corresponding()
        {
            var exception = Assert.Throws<ConstructorSearchException>(
                () =>
                {
                    this.kernel.Get(null, typeof(ObjectWithInjectedArguments));
                });

            Assert.Equal("Can create instance of ObjectWithInjectedArguments.", exception.Message);
        }

        [NamedFact(nameof(Should_return_null_When_binding_does_not_exists))]
        public void Should_return_null_When_binding_does_not_exists()
        {
            var actual = this.kernel.Get(null, typeof(IMotor));

            Assert.Null(actual);
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