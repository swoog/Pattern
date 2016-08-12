namespace Pattern.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Pattern.Core.Interfaces;

    internal class InternalFactory : IFactory
    {
        private readonly Type typeToCreate;

        private readonly Kernel kernel;

        public InternalFactory(Type typeToCreate, Kernel kernel)
        {
            this.typeToCreate = typeToCreate;
            this.kernel = kernel;
        }

        public object Create()
        {
            var constructor = this.typeToCreate.GetTypeInfo().GetConstructors().Single();

            var parameters = constructor.GetParameters().Select(arg => this.Resolve(arg, this.typeToCreate)).ToArray();

            return constructor.Invoke(parameters);
        }

        private object Resolve(ParameterInfo arg, Type typeToInject)
        {
            var callContext = new CallContext(arg.ParameterType, typeToInject);
            return this.kernel.GetInternal(callContext);
        }

    }
}