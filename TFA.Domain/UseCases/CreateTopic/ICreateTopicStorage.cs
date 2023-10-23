namespace TFA.Domain.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken);

    Task<Topic> CreateTopic(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken);
}