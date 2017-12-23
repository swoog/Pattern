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

        public virtual object Create(CallContext callContext)
        {
            var typeToCreate = this.TypeToCreate;

            if (typeToCreate.GetTypeInfo().IsGenericTypeDefinition)
            {
                typeToCreate = typeToCreate.MakeGenericType(callContext.GenericTypes);
            }

            var constructors = typeToCreate.GetTypeInfo().DeclaredConstructors.Select(CanResolve).ToList();

            var constructor = constructors
                .OrderByDescending(c => c.Parameters?.Count() ?? 0)
                .FirstOrDefault(c => c.Can);

            if (constructor != null)
            {
                var parameters = constructor.Parameters
                    .Select(arg => arg.Value ?? this.Resolve(arg.Type, typeToCreate)).ToArray();

                return constructor.Constructor.Invoke(parameters);
            }

            var typetoInject = constructors.FirstOrDefault(c => c.Parameters.All(p => p.IsInjectedType))?.Parameters?.FirstOrDefault(p => !p.Can && p.IsInjectedType);

            if (typetoInject != null)
            {
                throw new InjectionException(typetoInject.Type, typeToCreate);
            }

            throw new ConstructorSearchException(typeToCreate);
        }

        private ConstructorResult CanResolve(ConstructorInfo constructorInfo)
        {
            if (constructorInfo.IsStatic)
            {
                return new ConstructorResult { Can = false };
            }

            var parameters = constructorInfo.GetParameters();

            var parametersResult = parameters.Select(p => CanResolve(constructorInfo.DeclaringType, p)).ToList();

            return new ConstructorResult
            {
                Can = parametersResult.All(p => p.Can),
                Constructor = constructorInfo,
                Parameters = parametersResult,
            };
        }

        private ResolveResult CanResolve(Type parentType, ParameterInfo arg)
        {
            return
                ResolveResultStruct(arg, typeof(int)) ??
                ResolveResultStruct(arg, typeof(string)) ??
                ResolveResultStruct(arg, typeof(IntPtr)) ??
                new ResolveResult { Can = this.kernel.CanResolve(parentType, arg.ParameterType), Type = arg.ParameterType, IsInjectedType = true };
        }

        private static ResolveResult ResolveResultStruct(ParameterInfo arg, Type type)
        {
            if (arg.ParameterType == type)
            {
                return new ResolveResult { Can = false };
            }

            return null;
        }

        private object Resolve(Type parameterType, Type typeToInject)
        {
            return this.kernel.Get(typeToInject, parameterType);
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