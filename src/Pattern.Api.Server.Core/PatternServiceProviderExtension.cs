using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Pattern.Core.Interfaces;

namespace Pattern.Api.Server.Core
{
    public static class PatternServiceProviderExtension
    {
        public static IServiceProvider ToServiceProvider(this IKernel kernel, IEnumerable<ServiceDescriptor> services)
        {
            return new PaternServiceProvider(kernel, services);
        }
    }
}