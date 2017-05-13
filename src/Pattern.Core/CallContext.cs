namespace Pattern.Core
{
    using System;

    public class CallContext
    {
        public Type InstanciatedType { get; }

        public Type Parent { get; }

        public bool AutomaticInstance { get; }

        public Type EnumerableType { get; }

        public CallContext(Type instanciatedType, Type parent, bool automaticInstance = true, Type enumerableType = null)
        {
            this.InstanciatedType = instanciatedType;
            this.Parent = parent;
            this.AutomaticInstance = automaticInstance;
            this.EnumerableType = enumerableType;
        }

        public CallContext(Type instanciatedType)
        {
            this.InstanciatedType = instanciatedType;
        }
    }
}