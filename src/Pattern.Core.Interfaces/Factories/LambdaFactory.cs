namespace Pattern.Core.Interfaces.Factories
{
    using System;

    public class LambdaFactory : IFactory
    {
        private readonly Func<object> create;

        public LambdaFactory(Func<object> create)
        {
            this.create = create;
        }

        public object Create(object[] parameters)
        {
            return this.create();
        }
    }
}