namespace Pattern.Core
{
    using Interfaces;
    using System;
    using System.Collections.Generic;

    public class Kernel : IKernel
    {
        private readonly Dictionary<Type, IFactory> binds;

        public Kernel()
        {
            this.binds = new Dictionary<Type, IFactory>();
        }

        public void Bind(Type @from, Type to)
        {
            this.binds.Add(@from, new InternalFactory(to, this));
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            this.binds.Add(@from, toFactory);
        }

        public object Get(Type parentType, Type @from)
        {
            var callContext = new CallContext(@from, parentType);
            return this.GetInternal(callContext);
        }

        internal object GetInternal(CallContext callContext)
        {
            if (callContext.InstanciatedType == typeof(IKernel))
            {
                return this;
            }

            if (!this.binds.ContainsKey(callContext.InstanciatedType))
            {
                throw new InjectionException(callContext.InstanciatedType, callContext.Parent);
            }

            var to = this.binds[callContext.InstanciatedType];

            return to.Create();
        }
    }
}