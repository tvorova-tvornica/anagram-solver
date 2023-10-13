namespace AnagramSolver.BackgroundJobs;

public interface IJob<T>
{
    Task ExecuteAsync(T jobData);
}
