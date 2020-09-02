namespace Pattern.Core.Interfaces
{
    using System;

    public class InjectionException : Exception
    {
        public InjectionException(Type? @from, Type typeToInject)
            :base($"Injection not found for {from?.Name} when injected in {typeToInject?.Name}.")
        {
            
        }
    }
}