using Microsoft.AspNetCore.Http;
using Pattern.Core.Interfaces;

namespace Pattern.Api.Server.Core
{
    public class HttpSingletonFactory : IFactory
    {
        private readonly IFactory factory;

        private readonly IKernel kernel;

        public HttpSingletonFactory(IFactory factory, IKernel kernel)
        {
            this.factory = factory;
            this.kernel = kernel;
        }

        public object Create(CallContext callContext)
        {
            var name = callContext.InstanciatedType.FullName;

            var context = (IHttpContextAccessor)this.kernel.Get(null, typeof(IHttpContextAccessor));

            if (context?.HttpContext == null)
            {
                return this.factory.Create(callContext);
            }

            if (context.HttpContext.Items.ContainsKey(name))
            {
                return context.HttpContext.Items[name];
            }

            lock (context.HttpContext)
            {
                if (!context.HttpContext.Items.ContainsKey(name))
                {
                    var instance = this.factory.Create(callContext);

                    context.HttpContext.Items.Add(name, instance);
                }
            }

            return context.HttpContext.Items[name];
        }
    }
}