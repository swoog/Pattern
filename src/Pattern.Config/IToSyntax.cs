namespace Pattern.Config
{
    public interface IToSyntax<TFrom>
    {
        void ToSelf();

        void To<TTo>() where TTo : TFrom;

        void ToMethod<TTo>(System.Func<TTo> p) where TTo : TFrom;
    }
}