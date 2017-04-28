namespace Pattern.Web.Api
{
    using System.Web.Http;

    using Pattern.Core.Interfaces;

    public static class PatternDepency
    {
        public static IKernel UsePatternDependency<T>(this HttpConfiguration config)
            where T : IKernel, new()
        {
            var kernel = new T();
            config.DependencyResolver = new PatternDependencyResolver(kernel);
            return kernel;
        }
    }
}