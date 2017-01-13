using System;

namespace Pattern.Core.Interfaces.Factories
{
    public class ConstructiorSearchException : Exception
    {
        public ConstructiorSearchException()
        {
        }

        public ConstructiorSearchException(Type typeToCreate)
            :this($"Can create instance of {typeToCreate.Name}.")
        {
        }

        public ConstructiorSearchException(string message) : base(message)
        {
        }

        public ConstructiorSearchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}