namespace Pattern.Config
{
    public interface IToSyntax<TFrom>
    {
        void ToSelf();

        void To<TTo>() where TTo : TFrom;
    }
}