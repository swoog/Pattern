namespace Pattern.Core.Interfaces.Factories
{
    using System;
    using System.Collections.Generic;

    public class SingletonFactory : IFactory
    {
        private readonly IFactory factory;

        private Dictionary<Type, object> instance = new Dictionary<Type, object>();

        public SingletonFactory(IFactory factory)
        {
            this.factory = factory;
        }

        public object Create(CallContext callContext)
        {
            var key = this.GetKey(callContext);

            if (!this.instance.ContainsKey(key))
            {
                lock (this)
                {
                    if (!this.instance.ContainsKey(key))
                    {
                        this.instance.Add(key, this.factory.Create(callContext));
                    }
                }
            }

            return this.instance[key];
        }

        private Type GetKey(CallContext callContext)
        {
            if (callContext.GenericTypes != null && callContext.GenericTypes.Length == 1)
            {
                return callContext.GenericTypes[0];
            }

            return callContext.InstanciatedType;
        }
    }
}