namespace Pattern.Core
{
    using Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Pattern.Core.Interfaces.Factories;

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

        public bool CanResolve(Type parentType, Type @from)
        {
            var callContext = new CallContext(@from, parentType);

            var any = @from.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (any != null)
            {
                var constructorInfo = @from.GetTypeInfo().DeclaredConstructors.FirstOrDefault();

                if (constructorInfo == null)
                {
                    constructorInfo = typeof(List<>).MakeGenericType(any.GenericTypeArguments[0]).GetTypeInfo().DeclaredConstructors.First();
                }

                var list = constructorInfo.Invoke(null) as IList;

                callContext = new CallContext(any.GenericTypeArguments[0], parentType);
                return this.GetFactories(callContext).Count > 1;
            }

            return this.GetFactories(callContext).Count >= 1;
        }

        public object Get(Type parentType, Type @from, params object[] parameters)
        {
            var callContext = new CallContext(@from, parentType);
            if (callContext.InstanciatedType == typeof(IKernel))
            {
                return this;
            }

            var any = @from.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (any != null)
            {
                var constructorInfo = @from.GetTypeInfo().DeclaredConstructors.FirstOrDefault();

                if (constructorInfo == null)
                {
                    constructorInfo = typeof(List<>).MakeGenericType(any.GenericTypeArguments[0]).GetTypeInfo().DeclaredConstructors.First();
                }

                var list = constructorInfo.Invoke(null) as IList;

                callContext = new CallContext(any.GenericTypeArguments[0], parentType);
                var factories = this.GetFactories(callContext);
                foreach (var factory in factories)
                {
                    list.Add(factory.Create(parameters));
                }

                return list;
            }

            var to = this.GetFactories(callContext);

            if (to.Count > 1)
            {
                throw new FactoryException(callContext.InstanciatedType);
            }

            return to.Select(t => t.Create(parameters)).Single();
        }

        private IList<IFactory> GetFactories(CallContext callContext)
        {
            if (!this.binds.ContainsKey(callContext.InstanciatedType))
            {
                TypeInfo typeInfo = callContext.InstanciatedType.GetTypeInfo();
                if (typeInfo.IsClass && !typeInfo.IsAbstract)
                {
                    var factory = new TypeFactory(callContext.InstanciatedType, this);

                    this.binds.Add(callContext.InstanciatedType, new List<IFactory>() { factory });

                    return this.binds[callContext.InstanciatedType];
                }

                throw new InjectionException(callContext.InstanciatedType, callContext.Parent);
            }

            return this.binds[callContext.InstanciatedType];
        }
    }
}