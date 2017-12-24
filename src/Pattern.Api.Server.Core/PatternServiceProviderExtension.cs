using System;
using Microsoft.Extensions.DependencyInjection;
using Pattern.Core.Interfaces;

namespace Pattern.Api.Server.Core
{
    public static class PatternServiceProviderExtension
    {
        public static IServiceProvider ToServiceProvider(this IKernel kernel, IServiceCollection services)
        {
            return new PaternServiceProvider(kernel, services);
        }
    }
}