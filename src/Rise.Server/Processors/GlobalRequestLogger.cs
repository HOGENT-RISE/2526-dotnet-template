namespace Rise.Server.Processors;

public class GlobalRequestLogger : IGlobalPreProcessor
{
    public Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        Log.Information("Requesting '{RequestPath}' with parameters {@RequestParameters}", context.HttpContext.Request.Path, context.Request);

        return Task.CompletedTask;
    }
}