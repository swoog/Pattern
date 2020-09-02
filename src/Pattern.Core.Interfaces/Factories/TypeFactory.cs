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
        private readonly bool isGenericType;
        private readonly TypeInfo typeInfo;

        private ConstructorResult? constructor;

        public TypeFactory(Type typeToCreate, IKernel kernel)
        {
            this.TypeToCreate = typeToCreate;
            this.kernel = kernel;
            this.isGenericType = typeToCreate.GetTypeInfo().IsGenericTypeDefinition;
            this.typeInfo = this.TypeToCreate.GetTypeInfo();
        }

        public virtual object? Create(CallContext callContext)
        {
            var typeToCreate = this.typeInfo;

            if (this.isGenericType)
            {
                typeToCreate = this.TypeToCreate.MakeGenericType(callContext.GenericTypes).GetTypeInfo();

                var constructors = typeToCreate
                    .DeclaredConstructors
                    .Where(IsPublic)
                    .Select(CanResolve).ToArray();
                
                this.constructor = constructors
                    .OrderByDescending(c => c.Parameters.Length)
                    .FirstOrDefault(c => c.Can);
            }
            else if (this.constructor == null)
            {
                var constructors = typeToCreate
                    .DeclaredConstructors
                    .Where(IsPublic)
                    .Select(CanResolve).ToArray();
                
                this.constructor = constructors
                    .OrderByDescending(c => c.Parameters.Length)
                    .FirstOrDefault(c => c.Can);
            }

            if (this.constructor != null)
            {
                var parameters = this.constructor.Parameters
                    .Select(arg => this.Resolve(arg.Type ?? throw new NotSupportedException(), TypeToCreate)).ToArray();

                return this.constructor.Constructor?.Invoke(parameters);
            }

            var constructors2 = typeToCreate
                .DeclaredConstructors
                .Where(IsPublic)
                .Select(CanResolve).ToArray();
            
            var typetoInject = constructors2
                .FirstOrDefault(c => c.Parameters?.All(p => p.IsInjectedType) ?? false)
                ?.Parameters
                ?.FirstOrDefault(p => !p.Can && p.IsInjectedType);

            if (typetoInject != null)
            {
                throw new InjectionException(typetoInject.Type, TypeToCreate);
            }

            throw new ConstructorSearchException(TypeToCreate);
        }

        private static bool IsPublic(ConstructorInfo constructorInfo)
        {
            return constructorInfo.IsPublic;
        }

        private ConstructorResult CanResolve(ConstructorInfo constructorInfo)
        {
            if (constructorInfo.IsStatic)
            {
                return new ConstructorResult { Can = false };
            }

            var parameters = constructorInfo.GetParameters();

            var parametersResult = parameters
                .Select(p => CanResolve(constructorInfo.DeclaringType, p))
                .ToArray();

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
                ResolveResultStruct(arg, typeof(bool)) ??
                ResolveResultStruct(arg, typeof(IntPtr)) ??
                ResolveResultFunc(arg) ??
                new ResolveResult (arg.ParameterType) { Can = this.kernel.CanResolve(parentType, arg.ParameterType), IsInjectedType = true };
        }

        private static ResolveResult? ResolveResultStruct(ParameterInfo arg, Type type)
        {
            if (arg.ParameterType == type)
            {
                return new ResolveResult { Can = false };
            }

            return null;
        }

        private static ResolveResult? ResolveResultFunc(ParameterInfo arg)
        {
            if (arg.ParameterType.Name.StartsWith("Func"))
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
            public ResolveResult(Type type)
            {
                this.Type = type;
            }

            public ResolveResult()
            {
                this.Type = null;
            }

            public bool Can { get; set; }

            public Type? Type { get; }

            public bool IsInjectedType { get; set; }
        }

        public class ConstructorResult
        {
            public bool Can { get; set; }

            public ConstructorInfo? Constructor { get; set; }

            public ResolveResult[] Parameters { get; internal set; } = new ResolveResult[0];
        }
    }
}