using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;

namespace Pattern.Api.Server.Core
{
    public class PaternServiceProvider : IServiceProvider
    {
        public IKernel Kernel { get; }

        public PaternServiceProvider(IKernel kernel, IEnumerable<ServiceDescriptor> services)
        {
            this.Kernel = kernel;
            this.Kernel.Bind(typeof(IServiceProvider), new LambdaFactory(c => this));
            this.Kernel.Bind(typeof(IServiceScopeFactory), new LambdaFactory(c => new PatternServiceScope(this)));

            this.Populate(services);
        }

        public object GetService(Type serviceType)
        {
            var ty = serviceType.GetTypeInfo();
            return this.Kernel.Get(serviceType);
        }

        private void Populate(IEnumerable<ServiceDescriptor> services)
        {
            foreach (var service in services)
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Scoped:
                        this.Kernel.Bind(service.ServiceType, new HttpSingletonFactory(this.GetFactory(service), this.Kernel));
                        break;
                    case ServiceLifetime.Singleton:
                        this.Kernel.Bind(service.ServiceType, new SingletonFactory(this.GetFactory(service)));
                        break;
                    case ServiceLifetime.Transient:
                        this.Kernel.Bind(service.ServiceType, this.GetFactory(service));
                        break;
                }
            }
        }

        private IFactory GetFactory(ServiceDescriptor service)
        {
            if (service.ImplementationFactory != null)
            {
                return new LambdaFactory(c => service.ImplementationFactory(this));
            }

            if (service.ImplementationInstance != null)
            {
                return new LambdaFactory(c => service.ImplementationInstance);
            }

            return new TypeFactory(service.ImplementationType, this.Kernel);
        }
    }
}