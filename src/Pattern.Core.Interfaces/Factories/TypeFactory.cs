namespace Pattern.Core.Interfaces.Factories
{
    using System;
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

        public object Create()
        {
            var constructor = this.TypeToCreate.GetTypeInfo().GetConstructors().Single();

            var parameters = constructor.GetParameters().Select(arg => this.Resolve(arg, this.TypeToCreate)).ToArray();

            return constructor.Invoke(parameters);
        }

        private object Resolve(ParameterInfo arg, Type typeToInject)
        {
            return this.kernel.Get(typeToInject, arg.ParameterType);
        }
    }
}