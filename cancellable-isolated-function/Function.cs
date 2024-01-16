using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace cancellable_isolated_function
{
    public class Function
    {
        private readonly ILogger _logger;

        public Function(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function>();
        }

        [Function("cancel-me")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, CancellationToken cancellationToken)
        {
            try
            {
                var waitSeconds = 10;
                var waitTime = req.Query.GetValues("wait")?.FirstOrDefault();

                if(!string.IsNullOrEmpty(waitTime) && int.TryParse(waitTime, out waitSeconds))
                {
                    if (waitSeconds < 1 || waitSeconds > 30)
                    {
                        waitSeconds = 10;

                        _logger.LogWarning($"Invalid wait time: {waitTime}. Wait time should be between 1 and 30 seconds. Using default value: {waitSeconds} seconds");
                    }
                }
                
                _logger.LogInformation($"Waiting for {waitSeconds} seconds...");

                await Task.Delay(waitSeconds * 1000, cancellationToken);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                response.WriteString("Welcome to Azure Functions!");

                return response;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning($"Operation was cancelled. Reason: {ex.Message}");

                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
