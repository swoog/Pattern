namespace Pattern.Logging.Log4net
{
    using log4net;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;

    public static class Log4netBindingExtensions
    {
        public static IKernel BindLog4net(this IKernel kernel)
        {
            kernel.Bind(
                typeof(ILogger),
                new LambdaFactory(c => new Log4netLogger(LogManager.GetLogger(typeof(Log4netLogger)))));

            return kernel;
        }
    }
}