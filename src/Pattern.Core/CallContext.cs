namespace Pattern.Core
{
    using System;

    public class CallContext
    {
        public Type InstanciatedType { get; }

        public Type Parent { get; }

        public CallContext(Type instanciatedType, Type parent)
        {
            this.InstanciatedType = instanciatedType;
            this.Parent = parent;
        }

        public CallContext(Type instanciatedType)
        {
            this.InstanciatedType = instanciatedType;
        }
    }
}