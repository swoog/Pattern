namespace Pattern.Logging.Log4net
{
    using System;

    using log4net;

    using Pattern.Core.Interfaces;

    public class Log4netLogger : ILogger
    {
        public Log4netLogger()
        {
        }

        public void Debug(string message)
        {
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }
    }

    public static class Log4netBindingExtensions
    {
        public static IKernel BindLof4net(IKernel kernel)
        {
            kernel.Bind(typeof(Log4netLogger), typeof(Log4netLogger));
            //LogManager.GetLogger()

            return kernel;
        }
    }
}
