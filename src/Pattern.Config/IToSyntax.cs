namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public interface IToSyntax<TFrom>
    {
        IScopeSyntax ToSelf();

        IScopeSyntax To<TTo>() where TTo : TFrom;

        IScopeSyntax ToMethod<TTo>(System.Func<TTo> p) where TTo : TFrom;

        void ToFactory<T>() where T : class, IFactory;
    }
}