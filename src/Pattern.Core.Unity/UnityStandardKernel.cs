using System;

namespace Pattern.Core.Unity
{
    using Microsoft.Practices.Unity;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;

    public class UnityStandardKernel : IKernel
    {
        private readonly UnityContainer unityContainer;

        public UnityStandardKernel()
            : this(new UnityContainer())
        {

        }

        public UnityStandardKernel(UnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            switch (toFactory)
            {
                case TypeFactory typeFactory: this.unityContainer.RegisterType(@from, typeFactory.TypeToCreate);
                    break;
                default:
                    break;
            }
        }

        public object Get(Type parentType, Type @from, params object[] parameters)
        {
            return this.unityContainer.Resolve(@from);
        }

        public bool CanResolve(Type parentType, Type @from)
        {
            return this.unityContainer.IsRegistered(@from);
        }
    }
}
