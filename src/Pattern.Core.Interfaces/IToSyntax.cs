namespace Pattern.Core.Interfaces
{
    public interface IToSyntax<TFrom>
    {
        void ToSelf();

        void To<TTo>() where TTo : TFrom;
    }
}