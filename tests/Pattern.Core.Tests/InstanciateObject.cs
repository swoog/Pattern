namespace Pattern.Core.Tests
{
    using System.Collections.Generic;

    using Xunit;
    using Pattern.Core;
#if NET45
    using Pattern.Core.Ninject;
#endif
    using Pattern.Core.Interfaces;

    using Xunit.Extensions;

    public class InstanciateObject
    {
        public InstanciateObject()
        {
        }

        public static IEnumerable<object[]> Kernels => new[] { new object[]
                                                                   {
                                                                       new Kernel()
                                                                   },
#if NET45
            new object[]
                                                                   {
                                                                       new NinjectStandardKernel()
                                                                   }
#endif
        };

        [CustomTheory(DisplayName = nameof(Should_instanciate_type_When_bind_self_type)), MemberData("Kernels")]
        public void Should_instanciate_type_When_bind_self_type(IKernel kernel)
        {
            kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }
    }
}
