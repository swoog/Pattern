using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;

namespace Pattern.Logging.TraceWriter
{
    public static class TraceWriterBindingExtensions
    {
        public static IKernel BindTraceWriter(this IKernel kernel, Microsoft.Azure.WebJobs.Host.TraceWriter tracewriter)
        {
            kernel.Bind(
                typeof(ILogger),
                new LambdaFactory(c => new TraceWriterLogger(tracewriter)));

            return kernel;
        }
    }
}