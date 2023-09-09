namespace AnagramSolver.BackgroundJobs;

using Hangfire.Dashboard;

public class HangfireAuthFilter : IDashboardAuthorizationFilter
{
     public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}
