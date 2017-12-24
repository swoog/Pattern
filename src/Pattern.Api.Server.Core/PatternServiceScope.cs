using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Pattern.Api.Server.Core
{
    public class PatternServiceScope : Microsoft.Extensions.DependencyInjection.IServiceScopeFactory, IServiceScope
    {
        public PatternServiceScope(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new PatternServiceScope(this.ServiceProvider);
        }

        public void Dispose()
        {
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
