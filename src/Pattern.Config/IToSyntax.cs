namespace Pattern.Config
{
    using System;

    using Pattern.Core.Interfaces;

    public interface IToSyntax<TFrom>
    {
        IScopeSyntax ToSelf();

        IScopeSyntax To<TTo>() where TTo : TFrom;

        void ToMethod<TTo>(System.Func<TTo> p) where TTo : TFrom;

        void ToFactory<T>() where T : IFactory;
    }

    public interface IScopeSyntax
    {
        void InSingletonScope();

        void InScope(Func<IFactory, IFactory> factoryScope);
    }
}