namespace Pattern.Core.Interfaces
{
    using System;

    public interface IKernel
    {
        void Bind(Type @from, Type to);

        object Get(Type @from);
    }
}