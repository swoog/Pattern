namespace Pattern.Logging.Log4net
{
    using System;

    using log4net;

    public class Log4netLogger : ILogger
    {
        private readonly ILog log;

        public Log4netLogger(ILog log)
        {
            this.log = log;
        }

        public void Debug(string message)
        {
            this.log.Debug(message);
        }

        public void Info(string message)
        {
            this.log.Info(message);
        }

        public void Warning(string message)
        {
            this.log.Warn(message);
        }

        public void Error(string message)
        {
            this.log.Error(message);
        }
    }
}
