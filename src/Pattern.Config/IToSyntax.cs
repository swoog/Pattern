namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public interface IToSyntax<TFrom>
    {
        void ToSelf();

        void To<TTo>() where TTo : TFrom;

        void ToMethod<TTo>(System.Func<TTo> p) where TTo : TFrom;

        void ToFactory<T>() where T : IFactory;
    }
}