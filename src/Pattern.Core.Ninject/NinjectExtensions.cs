namespace Pattern.Core.Ninject
{
    using global::Ninject;

    public static class NinjectExtensions
    {
        public static Pattern.Core.Interfaces.IKernel BindPattern(IKernel ninjectKernel)
        {
            return new NinjectStandardKernel(ninjectKernel);
        }
    }
}