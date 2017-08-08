using NSubstitute;
using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;
using Pattern.Tests.Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Pattern.Module.Tests
{
    public class ModulesFeature
    {
        [NamedFact(nameof(Should_bind_module_when_add_module_to_kernel))]
        public void Should_bind_module_when_add_module_to_kernel()
        {
            var kernel = Substitute.For<IKernel>();

            kernel.LoadModule<FakeModule>();

            kernel.Received(1).Bind(typeof(IModule), Arg.Is<TypeFactory>(t => t.TypeToCreate == typeof(FakeModule)));
        }

        [NamedFact(nameof(Should_return_kernel_for_fluent_syntax_when_add_module_to_kernel))]
        public void Should_return_kernel_for_fluent_syntax_when_add_module_to_kernel()
        {
            var kernel = Substitute.For<IKernel>();

            var kernelWithModule = kernel.LoadModule<FakeModule>();

            Assert.Equal(kernel, kernelWithModule);
        }

        [NamedFact(nameof(Should_load_module_when_start_module_on_kernel))]
        public void Should_load_module_when_start_module_on_kernel()
        {
            var module = Substitute.For<IModule>();
            var kernel = Substitute.For<IKernel>();
            kernel.Get<List<IModule>>().Returns(new List<IModule> { module });

            kernel.StartModules();

            module.Received(1).Load(kernel);
        }

        [NamedFact(nameof(Should_return_kernel_for_fluent_syntax_when_start_module_on_kernel))]
        public void Should_return_kernel_for_fluent_syntax_when_start_module_on_kernel()
        {
            var module = Substitute.For<IModule>();
            var kernel = Substitute.For<IKernel>();
            kernel.Get<List<IModule>>().Returns(new List<IModule> { module });

            var kernelWithModule = kernel.StartModules();

            Assert.Equal(kernel, kernelWithModule);
        }
    }
}