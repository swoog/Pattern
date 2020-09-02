namespace Pattern.Core.Interfaces.Factories
{
    using System;

    public class LambdaFactory : IFactory
    {
        private readonly Func<CallContext, object?> create;

        public LambdaFactory(Func<CallContext, object?> create)
        {
            this.create = create;
        }

        public virtual object? Create(CallContext callContext)
        {
            return this.create(callContext);
        }
    }
}