using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFA.Domain.Exceptions;

namespace TFA.Api.Middlewares;

public static class ProblemDetailsFactoryExtension 
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext context, IntentionManagerException intentionManagerException)
    {
        return factory.CreateProblemDetails(context, StatusCodes.Status403Forbidden, "Authorization failed",
            detail: intentionManagerException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext context, DomainException domainException)
    {
        return factory.CreateProblemDetails(context, domainException.ErrorCode switch
        {
            ErrorCode.Gone => StatusCodes.Status410Gone, 
            _ => StatusCodes.Status500InternalServerError 
        }, domainException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext context, ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();
        
        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }

        return factory.CreateValidationProblemDetails(context, modelStateDictionary, StatusCodes.Status400BadRequest,
            detail: validationException.Message);
    }
}
