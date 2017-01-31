namespace Pattern.Core.Interfaces.Factories
{
    public class SingletonFactory : IFactory
    {
        private readonly IFactory factory;

        private object instance;

        public SingletonFactory(IFactory factory)
        {
            this.factory = factory;
        }

        public object Create(object[] parameters)
        {
            if (this.instance == null)
            {
                lock (this)
                {
                    if (this.instance == null)
                    {
                        this.instance = this.factory.Create(parameters);
                    }
                }
            }

            return this.instance;
        }
    }
}