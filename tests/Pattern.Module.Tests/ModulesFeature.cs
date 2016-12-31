using NSubstitute;
using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;
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
        [Fact]
        public void Should_bind_module_when_add_module_to_kernel()
        {
            var kernel = Substitute.For<IKernel>();

            kernel.LoadModule<FakeModule>();

            kernel.Bind(typeof(IModule), Arg.Is<TypeFactory>(t => t.TypeToCreate == typeof(FakeModule)));
        }

        [Fact]
        public void Should_load_module_when_start_module_on_kernel()
        {
            var module = Substitute.For<IModule>();
            var kernel = Substitute.For<IKernel>();
            kernel.Get<List<IModule>>().Returns(new List<IModule> { module });

            kernel.StartModules();

            module.Received(1).Load(kernel);
        }
    }
}