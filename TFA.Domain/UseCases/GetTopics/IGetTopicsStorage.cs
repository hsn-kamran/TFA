namespace TFA.Domain.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<Topic>, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken);

}
