namespace Pattern.Core.Interfaces
{
    using System;

    public class FactoryException : Exception
    {
        public FactoryException(Type instanciatedType)
            : base($"Injection have found many factories for {instanciatedType.Name}.")
        {
        }
        
        public FactoryException()
            : base($"Injection have not found any factory.")
        {
        }
    }
}