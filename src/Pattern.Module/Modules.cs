﻿using Pattern.Core.Interfaces;
using System.Collections.Generic;

namespace Pattern.Module
{
    public static class Modules
    {
        public static IKernel LoadModule<T>(this IKernel kernel)
            where T : IModule
        {
            kernel.Bind(typeof(IModule), new Pattern.Core.Interfaces.Factories.TypeFactory(typeof(T), kernel));

            return kernel;
        }

        public static IKernel StartModules(this IKernel kernel)
        {
            var modules = kernel.Get<List<IModule>>();

            foreach (var module in modules)
            {
                module.Load(kernel);
            }

            return kernel;
        }
    }
}
