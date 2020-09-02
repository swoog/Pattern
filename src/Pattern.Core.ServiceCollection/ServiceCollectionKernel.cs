using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;

namespace Pattern.Core.ServiceCollection
{
    public class ServiceCollectionKernel : IKernel
    {
        private readonly Microsoft.Extensions.DependencyInjection.ServiceCollection serviceCollection;

        private ServiceProvider serviceProvider;

        public ServiceCollectionKernel()
            :this(new Microsoft.Extensions.DependencyInjection.ServiceCollection())
        {
           
        }

        private ServiceCollectionKernel(Microsoft.Extensions.DependencyInjection.ServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            switch (toFactory)
            {
                case TypeFactory typeFactory:
                    this.serviceCollection
                        .AddTransient(@from, typeFactory.TypeToCreate);
                    break;
                default:
                    break;
            }
        }

        public void Init()
        {
            this.serviceProvider = this.serviceCollection.BuildServiceProvider();
        }

        public object Get(Type parentType, Type @from)
        {
            return this.serviceProvider.GetService(@from);
        }

        public bool CanResolve(Type parentType, Type @from)
        {
            return this.serviceProvider.GetService(@from) != null;
        }
    }
}