using Pattern.Core.Interfaces;
using System;

namespace Pattern.Core.Autofac
{
    using global::Autofac;

    using Pattern.Core.Interfaces.Factories;

    public class AutofacKernel : IKernel
    {
        private readonly ContainerBuilder containerBuilder;

        private IContainer container;

        public AutofacKernel()
            :this(new ContainerBuilder())
        {
           
        }

        private AutofacKernel(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            switch (toFactory)
            {
                case TypeFactory typeFactory:
                    this.containerBuilder
                        .RegisterType(typeFactory.TypeToCreate)
                        .As(@from);
                    break;
                default:
                    break;
            }
        }

        public void Init()
        {
            this.container = this.containerBuilder.Build();
        }

        public object Get(Type parentType, Type @from)
        {
            return this.container.Resolve(@from);
        }

        public bool CanResolve(Type parentType, Type @from)
        {
            return this.container.IsRegistered(@from);
        }
    }
}
