namespace Pattern.Core.Interfaces.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Pattern.Core.Interfaces;

    public class TypeFactory : IFactory
    {
        public Type TypeToCreate { get; }

        private readonly IKernel kernel;

        public TypeFactory(Type typeToCreate, IKernel kernel)
        {
            this.TypeToCreate = typeToCreate;
            this.kernel = kernel;
        }

        public virtual object Create(CallContext callContext, object[] parameters1)
        {
            var parameterQueue = new Queue<object>(parameters1);

            var typeToCreate = this.GetType();

            if (typeToCreate.GetTypeInfo().IsGenericType)
            {
                typeToCreate = typeToCreate.MakeGenericType(callContext.GenericTypes);
            }

            var constructors = typeToCreate.GetTypeInfo().DeclaredConstructors.Select(c => CanResolve(parameterQueue, c)).ToList();

            var constructor = constructors
                .OrderByDescending(c => c.Parameters?.Count() ?? 0)
                .FirstOrDefault(c => c.Can);

            if (constructor != null)
            {
                var parameters = constructor.Parameters
                    .Select(arg => arg.Value ?? this.Resolve(arg.Type, typeToCreate)).ToArray();

                return constructor.Constructor.Invoke(parameters);
            }

            var typetoInject = constructors.First().Parameters.FirstOrDefault(p => !p.Can && p.IsInjectedType);

            if (typetoInject != null)
            {
                throw new InjectionException(typetoInject.Type, typeToCreate);
            }

            throw new ConstructorSearchException(typeToCreate);
        }

        private Type GetType()
        {
            return this.TypeToCreate;
        }

        private ConstructorResult CanResolve(Queue<object> parameterQueue, ConstructorInfo constructorInfo)
        {
            if (constructorInfo.IsStatic)
            {
                return new ConstructorResult { Can = false };
            }

            var parameters = constructorInfo.GetParameters();

            var parametersResult = parameters.Select(p => CanResolve(parameterQueue, constructorInfo.DeclaringType, p)).ToList();

            return new ConstructorResult
            {
                Can = parametersResult.All(p => p.Can),
                Constructor = constructorInfo,
                Parameters = parametersResult,
            };
        }

        private ResolveResult CanResolve(Queue<object> parameterQueue, Type parentType, ParameterInfo arg)
        {
            return
                ResolveResultStruct(parameterQueue, arg, typeof(int)) ??
                ResolveResultStruct(parameterQueue, arg, typeof(string)) ??
                new ResolveResult { Can = this.kernel.CanResolve(parentType, arg.ParameterType), Type = arg.ParameterType, IsInjectedType = true };
        }

        private static ResolveResult ResolveResultStruct(Queue<object> parameterQueue, ParameterInfo arg, Type type)
        {
            if (arg.ParameterType == type)
            {
                object resolve;
                if (ResolveParameterQueue(arg, parameterQueue, out resolve))
                {
                    return new ResolveResult { Can = true, Value = resolve, Type = type };
                }

                return new ResolveResult { Can = false };
            }

            return null;
        }

        private object Resolve(Type parameterType, Type typeToInject)
        {
            return this.kernel.Get(typeToInject, parameterType);
        }

        private static bool ResolveParameterQueue(ParameterInfo arg, Queue<object> parameterQueue, out object resolve)
        {
            if (parameterQueue.Count != 0)
            {
                if (arg.ParameterType == parameterQueue.Peek().GetType())
                {
                    resolve = parameterQueue.Dequeue();
                    return true;
                }
            }

            resolve = null;

            return false;
        }

        public class ResolveResult
        {
            public bool Can { get; set; }
            public Type Type { get; internal set; }
            public object Value { get; set; }

            public bool IsInjectedType { get; set; }
        }

        public class ConstructorResult
        {
            public bool Can { get; set; }
            public ConstructorInfo Constructor { get; set; }
            public IEnumerable<ResolveResult> Parameters { get; internal set; }
        }
    }
}