using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Authorization.Extensions;
using TFA.Domain.Exceptions;


namespace TFA.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase  : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> validator;
    private readonly IIdentityProvider provider;
    private readonly IIntentionManager intentionManager;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(IValidator<CreateTopicCommand> validator, IIdentityProvider provider, IIntentionManager intentionManager, ICreateTopicStorage storage)
    {
        this.validator = validator;
        this.provider = provider;
        this.intentionManager = intentionManager;
        this.storage = storage;
    }


    public async Task<Topic> Execute(CreateTopicCommand createTopicCommand, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createTopicCommand, cancellationToken);

        var (forumId, title) = createTopicCommand;

        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(forumId, CancellationToken.None);

        if (!forumExists)
            throw new ForumNotFoundException(forumId);

        return await storage.CreateTopic(forumId, title, provider.Current.UserId, cancellationToken);
    }
}
        