namespace Pattern.Core.Interfaces
{
    using System;

    public static class KernelExtensions
    {
        public static object Get(this IKernel kernel, Type @from)
        {
            return kernel.Get(null, @from);
        }

        public static T Get<T>(this IKernel kernel)
        {
            return (T)kernel.Get(null, typeof(T));
        }
    }
}