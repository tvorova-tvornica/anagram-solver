using Sentry;

namespace AnagramSolver.Extensions;

public static class WebApplicationBuilderLoggingExtensions
{
    public static void AddAnagramSolverSentry(this WebApplicationBuilder builder)
    {
        var sentryDSN = Environment.GetEnvironmentVariable("SENTRY_DSN");

        if (sentryDSN is not null)
        {
            // This option is recommended. It enables Sentry's "Release Health" feature.
            var autoSessionTracking = true;
            // Enabling this option is recommended for client applications only. It ensures all threads use the same global scope.
            var isGlobalModeEnabled = false;
            // This option will enable Sentry's tracing features. You still need to start transactions and spans.
            var enableTracing = true;
            // Example sample rate for your transactions: captures 10% of transactions
            var tracesSampleRate = 0.1;

            SentrySdk.Init(options =>
            {
                options.Dsn = sentryDSN;
                options.AutoSessionTracking = autoSessionTracking;
                options.IsGlobalModeEnabled = isGlobalModeEnabled;
                options.EnableTracing = enableTracing;
                options.TracesSampleRate = tracesSampleRate;
            });

            builder.Logging.AddSentry(options =>
            {
                options.Dsn = sentryDSN;
                options.AutoSessionTracking = autoSessionTracking;
                options.IsGlobalModeEnabled = isGlobalModeEnabled;
                options.EnableTracing = enableTracing;
                options.TracesSampleRate = tracesSampleRate;
            });
        }
    }
}
