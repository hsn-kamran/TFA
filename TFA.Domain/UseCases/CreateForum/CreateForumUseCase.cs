using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Authorization.Extensions;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase : ICreateForumUseCase
{
    private readonly IValidator<CreateForumCommand> validator;
    private readonly IIdentityProvider provider;
    private readonly IIntentionManager intentionManager;
    private readonly ICreateForumStorage createForumStorage;

    public CreateForumUseCase(IValidator<CreateForumCommand> validator, IIdentityProvider provider, IIntentionManager intentionManager, ICreateForumStorage createForumStorage)
    {
        this.validator = validator;
        this.provider = provider;
        this.intentionManager = intentionManager;
        this.createForumStorage = createForumStorage;
    }

    public async Task Execute(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command);

        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        await createForumStorage.Create(command.Title, cancellationToken);
    }
}
