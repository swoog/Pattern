namespace Pattern.Core.Tests
{
    using System.Collections.Generic;

    using Xunit;
    using Pattern.Core;
    using Pattern.Core.Interfaces;

    using Xunit.Extensions;

    public class InstanciateObject
    {
        private IKernel kernel;

        public InstanciateObject()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_bind_self_type))]
        public void Should_instanciate_type_When_bind_self_type()
        {
            kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_type_When_inject_another_type))]
        public void Should_instanciate_type_When_inject_another_type()
        {
            this.kernel.Bind(typeof(ComplexClass), typeof(ComplexClass));
            this.kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = this.kernel.Get(typeof(ComplexClass));

            Assert.NotNull(instance);
            var complexType = Assert.IsType<ComplexClass>(instance);
            Assert.NotNull(complexType.InjectedType);
        }
    }

    public class ComplexClass
    {
        public ComplexClass(SimpleClass injectedType)
        {
            this.InjectedType = injectedType;
        }

        public SimpleClass InjectedType { get; set; }
    }
}
