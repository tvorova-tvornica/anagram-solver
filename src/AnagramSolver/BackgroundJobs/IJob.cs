namespace AnagramSolver.BackgroundJobs;

public interface IJob<TJobData>
{
    Task ExecuteAsync(TJobData jobData);
}
