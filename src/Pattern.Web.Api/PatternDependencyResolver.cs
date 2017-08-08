namespace Pattern.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;

    using Pattern.Core.Interfaces;

    public class PatternDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public PatternDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return this.GetInstance(serviceType);
        }

        private object GetInstance(Type serviceType)
        {
            try
            {
                return this.kernel.Get(serviceType);
            }
            catch (InjectionException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.kernel.Get(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
            }
            catch (InjectionException)
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}