﻿namespace Pattern.Core
{
    using Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Kernel : IKernel
    {
        private readonly Dictionary<Type, IList<IFactory>> binds;

        public Kernel()
        {
            this.binds = new Dictionary<Type, IList<IFactory>>();
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            if (!this.binds.ContainsKey(@from))
            {
                this.binds.Add(@from, new List<IFactory>() { toFactory });
            }
            else
            {
                this.binds[@from].Add(toFactory);
            }
        }

        public object Get(Type parentType, Type @from)
        {
            var callContext = new CallContext(@from, parentType);
            if (callContext.InstanciatedType == typeof(IKernel))
            {
                return this;
            }

            var any = @from.GetTypeInfo().GetInterfaces().FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (any != null)
            {
                var list = @from.GetTypeInfo().GetConstructors().First().Invoke(null) as IList;

                callContext = new CallContext(any.GenericTypeArguments[0], parentType);
                var factories = this.GetFactories(callContext);
                foreach (var factory in factories)
                {
                    list.Add(factory.Create());
                }

                return list;
            }

            var to = this.GetFactories(callContext);

            return to.Select(t => t.Create()).First();
        }

        private IList<IFactory> GetFactories(CallContext callContext)
        {
            if (!this.binds.ContainsKey(callContext.InstanciatedType))
            {
                throw new InjectionException(callContext.InstanciatedType, callContext.Parent);
            }

            var to = this.binds[callContext.InstanciatedType];
            return to;
        }
    }
}