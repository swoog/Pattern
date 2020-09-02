using Microsoft.Extensions.DependencyInjection;

namespace Pattern.Perf
{
    public class ServiceCollectionPerfAnalyser : IPerfAnalyser
    {
        private ServiceCollection serviceCollection;
        private ServiceProvider serviceProvider;

        public void Create()
        {
            this.serviceCollection = new ServiceCollection();
        }

        public void Bind()
        {
            this.serviceCollection.AddTransient<IMyClass, MyClass>();
            this.serviceCollection.AddTransient<IMyClassGeneric<object>, MyClassGeneric<object>>();
        }

        public void Init()
        {
            this.serviceProvider = this.serviceCollection.BuildServiceProvider();
        }

        public void Get()
        {
            this.serviceProvider.GetService<IMyClass>();
        }
        
        public string Name => "ServiceCollection";
    }
}