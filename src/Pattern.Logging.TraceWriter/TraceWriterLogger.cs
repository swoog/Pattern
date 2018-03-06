using System;

namespace Pattern.Logging.TraceWriter
{
    public class TraceWriterLogger : ILogger
    {
        private readonly Microsoft.Azure.WebJobs.Host.TraceWriter log;

        public TraceWriterLogger(Microsoft.Azure.WebJobs.Host.TraceWriter log)
        {
            this.log = log;
        }

        public void Debug(string message)
        {
            this.log.Verbose(message);
        }

        public void Info(string message)
        {
            this.log.Info(message);
        }

        public void Warning(string message)
        {
            this.log.Warning(message);
        }

        public void Error(string message)
        {
            this.log.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            this.log.Error(message, ex);
        }
    }
}
