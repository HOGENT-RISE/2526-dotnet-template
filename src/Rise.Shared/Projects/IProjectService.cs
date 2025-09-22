namespace Rise.Shared.Projects;

public interface IProjectService
{
    Task<Result> EditAsync(ProjectRequest.Edit req, CancellationToken ctx);
}