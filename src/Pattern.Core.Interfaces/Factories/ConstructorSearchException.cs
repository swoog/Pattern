using System;

namespace Pattern.Core.Interfaces.Factories
{
    public class ConstructorSearchException : Exception
    {
        public ConstructorSearchException()
        {
        }

        public ConstructorSearchException(Type typeToCreate)
            :this($"Can not create instance of {typeToCreate.Name}.")
        {
        }

        public ConstructorSearchException(string message) : base(message)
        {
        }

        public ConstructorSearchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}