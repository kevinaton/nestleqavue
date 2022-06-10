using Microsoft.Extensions.Logging;

namespace HRD.WebApi.Logging
{
    public class ApplicationLogging
    {

        public static void ConfigureLogger(ILogger logger)
        {
            Logger = logger;
        }

        public static ILogger Logger { get; set; }
        public ApplicationLogging(ILogger<ApplicationLogging> ilogger)
        {
            Logger = ilogger;
        }
    }
}
