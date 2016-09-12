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

        public object Create(object[] parameters1)
        {
            var constructor = this.TypeToCreate.GetTypeInfo().DeclaredConstructors.Single();

            var parameterQueue = new Queue<object>(parameters1);

            var parameters = constructor.GetParameters().Select(arg => this.Resolve(arg, this.TypeToCreate, parameterQueue)).ToArray();

            return constructor.Invoke(parameters);
        }

        private object Resolve(ParameterInfo arg, Type typeToInject, Queue<object> parameterQueue)
        {
            var parameterType = arg.ParameterType;

            if (parameterType == typeof(int))
            {
                object resolve;
                if (ResolveParameterQueue(arg, parameterQueue, out resolve)) return resolve;
            }
            else if (parameterType == typeof(string))
            {
                object resolve;
                if (ResolveParameterQueue(arg, parameterQueue, out resolve)) return resolve;
            }

            return this.kernel.Get(typeToInject, arg.ParameterType);
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
    }
}