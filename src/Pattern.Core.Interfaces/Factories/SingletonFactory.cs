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

        public object Create(CallContext callContext, object[] parameters)
        {
            if (this.instance == null)
            {
                lock (this)
                {
                    if (this.instance == null)
                    {
                        this.instance = this.factory.Create(callContext, parameters);
                    }
                }
            }

            return this.instance;
        }
    }
}