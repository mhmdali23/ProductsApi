using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebAppApi.Filters
{
    public class LogActivityFilter : IActionFilter
    {
        private readonly ILogger<LogActivityFilter> logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation
                            ($"Executing action {context.ActionDescriptor.DisplayName} on controller {context.Controller}" +
                            $"with args {JsonSerializer.Serialize(context.ActionArguments)}");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation($"action {context.ActionDescriptor.DisplayName} Finished");
        }
       
    }
}
