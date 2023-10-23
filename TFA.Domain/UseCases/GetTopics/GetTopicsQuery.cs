namespace TFA.Domain.UseCases.GetTopics;

public record class GetTopicsQuery(Guid ForumId, int Skip, int Take);
