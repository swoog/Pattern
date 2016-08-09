namespace Pattern.Core
{
    using System;
    using System.Collections.Generic;
    using Pattern.Core.Interfaces;

    public class Kernel : IKernel
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