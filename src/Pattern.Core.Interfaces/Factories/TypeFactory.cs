namespace Pattern.Core.Interfaces.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypeFactory : IFactory
    {
        public Type TypeToCreate { get; }

        private readonly IKernel kernel;

        public TypeFactory(Type typeToCreate, IKernel kernel)
        {
            this.TypeToCreate = typeToCreate;
            this.kernel = kernel;
        }

        public virtual object Create(object[] parameters1)
        {
            var parameterQueue = new Queue<object>(parameters1);

            var constructors = this.TypeToCreate.GetTypeInfo().DeclaredConstructors.Select(c => CanResolve(parameterQueue, c)).ToList();

            var constructor = constructors
                .OrderByDescending(c => c.Parameters?.Count() ?? 0)
                .FirstOrDefault(c => c.Can);

            if (constructor == null)
            {
                throw new ConstructorSearchException(this.TypeToCreate);
            }

            var parameters = constructor.Parameters
                .Select(arg => arg.Value ?? this.Resolve(arg.Type, this.TypeToCreate, parameterQueue)).ToArray();

            return constructor.Constructor.Invoke(parameters);
        }

        private ConstructorResult CanResolve(Queue<object> parameterQueue, ConstructorInfo constructorInfo)
        {
            if (constructorInfo.IsStatic)
            {
                return new ConstructorResult() { Can = false };
            }

            var parameters = constructorInfo.GetParameters();

            var parametersResult = parameters.Select(p => CanResolve(parameterQueue, constructorInfo.DeclaringType, p)).ToList();

            return new ConstructorResult()
            {
                Can = parametersResult.All(p => p.Can),
                Constructor = constructorInfo,
                Parameters = parametersResult,
            };
        }

        public class ResolveResult
        {
            public bool Can { get; set; }
            public Type Type { get; internal set; }
            public object Value { get; set; }
        }

        private ResolveResult CanResolve(Queue<object> parameterQueue, Type parentType, ParameterInfo arg)
        {
            if (arg.ParameterType == typeof(int))
            {
                object resolve;
                if (ResolveParameterQueue(arg, parameterQueue, out resolve))
                {
                    return new ResolveResult() { Can = true, Value = resolve, Type = typeof(int) };
                }

                return new ResolveResult() { Can = false };
            }
            else if (arg.ParameterType == typeof(string))
            {
                object resolve;
                if (ResolveParameterQueue(arg, parameterQueue, out resolve))
                {
                    return new ResolveResult() { Can = true, Value = resolve, Type = typeof(string) };
                }

                return new ResolveResult() { Can = false };
            }

            return new ResolveResult() { Can = this.kernel.CanResolve(parentType, arg.ParameterType), Type = arg.ParameterType };
        }

        private object Resolve(Type parameterType, Type typeToInject, Queue<object> parameterQueue)
        {
            return this.kernel.Get(typeToInject, parameterType);
        }

        private static bool ResolveParameterQueue(ParameterInfo arg, Queue<object> parameterQueue, out object resolve)
        {
            if (parameterQueue.Count != 0)
            {
                if (arg.ParameterType == parameterQueue.Peek().GetType())
                {
                    {
                        resolve = parameterQueue.Dequeue();
                        return true;
                    }
                }
            }

            resolve = null;

            return false;
        }

        public class ConstructorResult
        {
            public bool Can { get; set; }
            public ConstructorInfo Constructor { get; set; }
            public IEnumerable<ResolveResult> Parameters { get; internal set; }
        }
    }
}