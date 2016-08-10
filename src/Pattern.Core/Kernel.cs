namespace Pattern.Core
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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
            var callContext = new CallContext(@from);
            return this.GetInternal(callContext);
        }

        private object GetInternal(CallContext callContext)
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

            var constructor = to.GetTypeInfo().GetConstructors().Single();

            var parameters = constructor.GetParameters().Select(arg => this.Resolve(arg, to)).ToArray();

            return constructor.Invoke(parameters);
        }

        private object Resolve(ParameterInfo arg, Type typeToInject)
        {
            var callContext = new CallContext(arg.ParameterType, typeToInject);
            return this.GetInternal(callContext);
        }
    }
}