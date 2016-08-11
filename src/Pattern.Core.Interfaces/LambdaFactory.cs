namespace Pattern.Core.Interfaces
{
    using System;

    public class LambdaFactory : IFactory
    {
        private readonly Func<object> create;

        public LambdaFactory(Func<object> create)
        {
            this.create = create;
        }

        public object Create()
        {
            return this.create();
        }
    }
}