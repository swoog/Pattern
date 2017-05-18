namespace Pattern.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Interfaces;

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
            var callContext = CreateCallContext(parentType, @from);

            if (callContext.EnumerableType != null)
            {
                return true;
            }

            if (callContext.InstanciatedType == typeof(IKernel))
            {
                return true;
            }

            return this.GetFactories(callContext).Count >= 1;
        }

        public object Get(Type parentType, Type @from, params object[] parameters)
        {
            if (@from == null)
            {
                throw new ArgumentException("From type cannot be null");
            }

            var callContext = CreateCallContext(parentType, @from);

            if (callContext.InstanciatedType == typeof(IKernel))
            {
                return this;
            }

            var factories = this.GetFactories(callContext);

            var instanciateValues = factories.Select(t => t.Create(parameters));
            if (callContext.EnumerableType != null)
            {
                var list = CreateList(callContext);

                foreach (var value in instanciateValues)
                {
                    list.Add(value);
                }

                return list;
            }

            if (factories.Count > 1)
            {
                throw new FactoryException(callContext.InstanciatedType);
            }

            if (factories.Count == 0)
            {
                return null;
            }

            return instanciateValues.Single();
        }

        private static IList CreateList(CallContext callContext)
        {
            var constructorInfo = GetConstructorInfo(callContext.EnumerableType) 
                ?? GetConstructorInfo(typeof(List<>).MakeGenericType(callContext.InstanciatedType));

            return constructorInfo.Invoke(null) as IList;
        }

        private static ConstructorInfo GetConstructorInfo(Type enumerableType)
        {
            return enumerableType.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
        }

        private static CallContext CreateCallContext(Type parentType, Type @from)
        {
            var callContext = new CallContext(@from, parentType);

            var typeInfo = @from.GetTypeInfo();
            var enumerableType = GetEnumerableType(typeInfo.ImplementedInterfaces);
            if (enumerableType == null && IsEnumerable(typeInfo))
            {
                enumerableType = @from;
            }

            if (enumerableType != null)
            {
                callContext = new CallContext(enumerableType.GenericTypeArguments[0], parentType, false, enumerableType);
            }

            return callContext;
        }

        private bool ContainsFactory(CallContext callContext)
        {
            return this.binds.ContainsKey(callContext.InstanciatedType);
        }

        private static Type GetEnumerableType(IEnumerable<Type> typeInfoImplementedInterfaces)
        {
            return typeInfoImplementedInterfaces.FirstOrDefault(t => IsEnumerable(t.GetTypeInfo()));
        }

        private static bool IsEnumerable(TypeInfo t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private IList<IFactory> GetFactories(CallContext callContext)
        {
            if (!this.ContainsFactory(callContext))
            {
                var typeInfo = callContext.InstanciatedType.GetTypeInfo();
                if (!typeInfo.IsClass || typeInfo.IsAbstract || !callContext.AutomaticInstance)
                {
                    return new List<IFactory>();
                }

                this.Bind(callContext.InstanciatedType, new TypeFactory(callContext.InstanciatedType, this));
            }

            return this.binds[callContext.InstanciatedType];
        }
    }
}