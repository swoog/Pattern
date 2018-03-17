using System;
using Microsoft.ApplicationInsights;

namespace Pattern.Logging.ApplicationInsights
{
    public class ApplicationInsights : ILogger
    {
        private TelemetryClient telemetryClient;

        public ApplicationInsights()
        {
            telemetryClient = new TelemetryClient();
        }

        public void Debug(string message)
        {
            telemetryClient.TrackException();
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

        public void Error(Exception ex, string message)
        {
            throw new NotImplementedException();
        }
    }
}
