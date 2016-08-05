namespace Pattern.Core.Tests
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public class InstanciateObject
    {
        public InstanciateObject()
        {
        }

        [Fact]
        public void Should_instanciate_type_When_bind_self_type()
        {
            var kernel = new Kernel();

            kernel.Bind(typeof(SimpleClass), typeof(SimpleClass));

            var instance = kernel.Get(typeof(SimpleClass));

            Assert.NotNull(instance);
            Assert.IsType<SimpleClass>(instance);
        }
    }

    public class SimpleClass
    {
    }

    public class Kernel
    {
        private readonly Dictionary<Type, Type> binds;

        public Kernel()
        {
            this.binds = new Dictionary<Type, Type>();
        }

        public void Bind(Type @from, Type to)
        {
            this.binds.Add(@from, to);
        }

        public object Get(Type @from)
        {
            var to = this.binds[@from];

            return Activator.CreateInstance(to);
        }
    }
}
