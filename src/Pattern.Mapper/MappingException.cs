namespace Pattern.Mapper
{
    using System;

    public class MappingException : Exception
    {
        public MappingException(string message)
            : base(message)
        {

        }
    }
}