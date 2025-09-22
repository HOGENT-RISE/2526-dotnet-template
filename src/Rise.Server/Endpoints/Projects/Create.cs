using Rise.Shared.Products;
using Rise.Shared.Projects;

namespace Rise.Server.Endpoints.Products;

public class Edit(IProjectService projectService) : Endpoint<ProjectRequest.Edit, Result>
{
    public override void Configure()
    {
        Put("/api/projects");
    }

    public override Task<Result> ExecuteAsync(ProjectRequest.Edit req, CancellationToken ctx)
    {
        return projectService.EditAsync(req, ctx);
    }
}