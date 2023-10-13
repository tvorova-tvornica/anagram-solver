using Sentry;

namespace AnagramSolver.BackgroundJobs;

public class TracingJobDecorator<T> : IJob<T>
{
    private readonly IJob<T> _decorated;
    private readonly Func<IHub> _getHub;

    public TracingJobDecorator(
        IJob<T> decorated,
        Func<IHub> getHub)
    {
        _decorated = decorated;
        _getHub = getHub;
    }

    public async Task ExecuteAsync(T jobData)
    {
        var hub = _getHub();
        
        if (!hub.IsEnabled)
        {
            await _decorated.ExecuteAsync(jobData).ConfigureAwait(false);
            return;
        }

        using (hub.PushAndLockScope())
        {
            var transaction = hub.StartTransaction($"BackgroundJob {_decorated.GetType().Name}", "hangfire.server");
            
            hub.ConfigureScope(scope =>
            {
                scope.Transaction = transaction;
            });

            Exception? exception = null;

            try
            {
                await _decorated.ExecuteAsync(jobData).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                if (exception is null)
                {
                    transaction.Finish(SpanStatus.Ok);
                }
                else
                {
                    transaction.Finish(exception);
                }
            }
        }
    }
}
