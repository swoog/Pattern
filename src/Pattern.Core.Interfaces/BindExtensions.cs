namespace Pattern.Core.Interfaces
{
    public static class BindExtensions
    {
        public static IToSyntax<T> Bind<T>(this IKernel kernel)
        {
            return new ToSyntax<T>(kernel);
        }
    }
}