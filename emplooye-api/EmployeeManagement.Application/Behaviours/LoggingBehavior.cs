using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Behaviours;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestNameWithGuid = $"{request.GetType().Name} [{Guid.NewGuid().ToString()}]";
          
        logger.LogInformation($"[START] {requestNameWithGuid}");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            LogRequestWithProps(request, requestNameWithGuid);

            return await next();
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation($"[END] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }
    }

    private void LogRequestWithProps(TRequest request, string requestNameWithGuid)
    {
        try
        {
            logger.LogInformation($"[PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
        }
        catch (NotSupportedException)
        {
            logger.LogInformation($"[Serialization ERROR] {requestNameWithGuid} Could not serialize the request.");
        }
    }
}