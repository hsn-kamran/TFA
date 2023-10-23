using FluentValidation;

namespace TFA.Domain.UseCases.GetTopics;

internal class GetTopicsUseCase : IGetTopicsUseCase
{
    private readonly IValidator<GetTopicsQuery> validator;
    private readonly IGetTopicsStorage getTopicsStorage;

    public GetTopicsUseCase(IValidator<GetTopicsQuery> validator, IGetTopicsStorage getTopicsStorage)
    {
        this.validator = validator;
        this.getTopicsStorage = getTopicsStorage;
    }

    public async Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);

        var (forumId, skip, take) = query;

        return await getTopicsStorage.GetTopics(forumId, skip, take, cancellationToken);
    }
}