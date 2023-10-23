using FluentValidation;

namespace TFA.Domain.UseCases.GetTopics;

internal class GetTopicsQueryValidator : AbstractValidator<GetTopicsQuery>
{
    public GetTopicsQueryValidator()
    {
        RuleFor(q => q.ForumId).NotEmpty();
        RuleFor(q => q.Take).GreaterThanOrEqualTo(0);
        RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
    }
}
