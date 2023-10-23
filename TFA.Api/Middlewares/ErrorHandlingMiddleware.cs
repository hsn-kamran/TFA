using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Domain.Exceptions;

namespace TFA.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ProblemDetailsFactory problemDetailsFactory)
        {
            try
            {
                //logger.LogInformation("Executing pipeline, requested url {RequestedUrl} at {ShortDateTime}", context.Request.Path, DateTime.Now.ToShortDateString());

                await _next.Invoke(context);                
            }
            catch (Exception exception)
            {
                logger.LogInformation("Exception {Message} at {DateTime}", exception.Message, DateTime.Now.ToString());
                
                var httpStatucCode = exception switch
                {
                    IntentionManagerException => StatusCodes.Status403Forbidden,
                    ValidationException => StatusCodes.Status400BadRequest,
                    DomainException domainException => domainException switch
                    {
                        ForumNotFoundException => StatusCodes.Status410Gone,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    _ => StatusCodes.Status500InternalServerError
                };

                var problemDetails = exception switch
                {
                    IntentionManagerException intentionManagerException => problemDetailsFactory.CreateFrom(context, intentionManagerException),
                    ValidationException validationException => problemDetailsFactory.CreateFrom(context, validationException),
                    DomainException domainException => problemDetailsFactory.CreateFrom(context, domainException), 
                    _ => problemDetailsFactory.CreateProblemDetails(context, httpStatucCode, "Unhandled error, please contact us!", detail: exception.Message)
                };

                context.Response.StatusCode = httpStatucCode;

                if(problemDetails is ValidationProblemDetails validationProblemDetails)
                    await context.Response.WriteAsJsonAsync(validationProblemDetails);

                if (problemDetails is ProblemDetails)
                    await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

    }
}
