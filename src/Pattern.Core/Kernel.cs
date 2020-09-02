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
            this.Bind(typeof(IKernel), new LambdaFactory(c => this));
        }

        public Action<CallContext> Log { get; set; }

        public void Bind(Type @from, IFactory toFactory)
        {
            if (!this.binds.ContainsKey(@from))
            {
                this.binds.Add(@from, new List<IFactory> { toFactory });
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

            return this.GetFactories(ref callContext).Count >= 1;
        }

        public object Get(Type parentType, Type @from)
        {
            if (@from == null)
            {
                throw new ArgumentException("From type cannot be null");
            }

            var callContext = CreateCallContext(parentType, @from);

            this.Log?.Invoke(callContext);

            var factories = this.GetFactories(ref callContext);

            if (callContext.EnumerableType != null)
            {
                var instanciateValues = factories.Select(t => t.Create(callContext));
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

            return factories[0].Create(callContext);
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

        private Dictionary<string, CallContext> callContextCache = new Dictionary<string, CallContext>();

        private CallContext CreateCallContext(Type parentType, Type @from)
        {
            var key = $"{parentType?.FullName}_{@from.FullName}";

            if (this.callContextCache.ContainsKey(key))
            {
                return this.callContextCache[key];
            }

            var callContext = CreateInternalCallContext(parentType, @from);

            this.callContextCache.Add(key, callContext);

            return callContext;
        }

        private CallContext CreateInternalCallContext(Type parentType, Type @from)
        {
            var typeInfo = @from.GetTypeInfo();
            var enumerableType = GetEnumerableType(typeInfo.ImplementedInterfaces);
            if (enumerableType != null && !this.binds.ContainsKey(@from))
            {
                return new CallContext(enumerableType.GenericTypeArguments[0], parentType, false, enumerableType);
            }

            if (IsEnumerable(typeInfo))
            {
                return new CallContext(@from.GenericTypeArguments[0], parentType, false, @from);
            }

            return new CallContext(@from, parentType);
        }

        private bool ContainsFactory(CallContext callContext)
        {
            return this.binds.ContainsKey(callContext.InstanciatedType);
        }

        private static Type GetEnumerableType(IEnumerable<Type> typeInfoImplementedInterfaces)
        {
            foreach (var typeInfoImplementedInterface in typeInfoImplementedInterfaces)
            {
                if (IsEnumerable(typeInfoImplementedInterface.GetTypeInfo()))
                {
                    return typeInfoImplementedInterface;
                }
            }

            return default(Type);
        }

        private static bool IsEnumerable(TypeInfo t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private IList<IFactory> GetFactories(ref CallContext callContext)
        {
            if (this.ContainsFactory(callContext))
            {
                return this.binds[callContext.InstanciatedType];
            }

            var typeInfo = callContext.InstanciatedType.GetTypeInfo();
            if (!typeInfo.IsClass || typeInfo.IsAbstract || !callContext.AutomaticInstance)
            {
                if (typeInfo.IsInterface && typeInfo.IsGenericType && callContext.EnumerableType == null)
                {
                    callContext = new CallContext(
                        typeInfo.GetGenericTypeDefinition(),
                        callContext.Parent,
                        callContext.AutomaticInstance,
                        callContext.EnumerableType,
                        genericTypes: typeInfo.GenericTypeArguments);

                    if (this.ContainsFactory(callContext))
                    {
                        return this.binds[callContext.InstanciatedType];
                    }
                }

                return new List<IFactory>();
            }

            this.Bind(callContext.InstanciatedType, new TypeFactory(callContext.InstanciatedType, this));

            return this.binds[callContext.InstanciatedType];
        }
    }
}