using Microsoft.EntityFrameworkCore;
using Rise.Domain.Projects;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Identity;
using Rise.Shared.Projects;

namespace Rise.Services.Projects;

public class ProjectService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : IProjectService
{
    public async Task<Result> EditAsync(ProjectRequest.Edit req, CancellationToken ctx)
    {
        Project? p = await dbContext.Projects
            .Include(x => x.Technician) // JOIN in SQL without this, the tech will always blank
            .SingleOrDefaultAsync(x => x.Id == req.ProjectId, ctx);
        
        if(p is null)
            return Result.NotFound($"Project with Id '{req.ProjectId}' was not found.");
        
        Technician loggedInTechnician = await dbContext.Technicians.SingleAsync(x => x.AccountId == sessionContextProvider.User.GetUserId(), ctx);

        if (!p.CanBeEditedBy(loggedInTechnician))
            return Result.Unauthorized("You are not authorized to edit this project.");
        
        p.Edit(req.Name);

        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }   
}