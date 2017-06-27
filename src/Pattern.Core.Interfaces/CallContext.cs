namespace Pattern.Core.Interfaces
{
    using System;

    public class CallContext
    {
        public Type[] GenericTypes { get; }

        public Type InstanciatedType { get; }

        public Type Parent { get; }

        public bool AutomaticInstance { get; }

        public Type EnumerableType { get; }

        public CallContext(
            Type instanciatedType,
            Type parent,
            bool automaticInstance = true,
            Type enumerableType = null,
            Type[] genericTypes = null)
        {
            this.InstanciatedType = instanciatedType;
            this.Parent = parent;
            this.AutomaticInstance = automaticInstance;
            this.EnumerableType = enumerableType;
            this.GenericTypes = genericTypes;
        }
    }
}