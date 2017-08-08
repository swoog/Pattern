namespace Pattern.Core.Interfaces
{
    using System;

    public interface IKernel
    {
        void Bind(Type @from, IFactory toFactory);

        object Get(Type parentType, Type @from, params object[] parameters);

        bool CanResolve(Type parentType, Type @from);
    }
}