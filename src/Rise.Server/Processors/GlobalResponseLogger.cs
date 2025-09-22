namespace Rise.Server.Processors;

public class GlobalResponseLogger : IGlobalPostProcessor
{
    public Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        Log.Information("Requested '{RequestPath}' with result {@RequestResult}", context.HttpContext.Request.Path, context.Response);

        return Task.CompletedTask;
    }
}