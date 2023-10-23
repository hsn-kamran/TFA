using FluentValidation;
using TFA.Domain.Exceptions;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(50).WithErrorCode(ValidationErrorCode.NotEmpty);
    }
}
