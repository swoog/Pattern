namespace Pattern.Core.Interfaces
{
    using System;

    public interface IKernel
    {
        void Bind(Type @from, Type to);

        void Bind(Type @from, IFactory toFactory);

        object Get(Type @from);
    }
}