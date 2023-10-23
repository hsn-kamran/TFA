using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests.CreateTopic;

public class CreateTopicUseCaseShould
{
    private readonly ISetup<IValidator<CreateTopicCommand>, ValidationResult> createTopicCommandValidatorSetup;
    private readonly CreateTopicUseCase sut;

    private readonly Mock<ICreateTopicStorage> createTopicStorage;
    private readonly ISetup<ICreateTopicStorage, Task<bool>> forumExistsSetup;
    private readonly ISetup<ICreateTopicStorage, Task<Models.Topic>> createTopicSetup;
    private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;

    public CreateTopicUseCaseShould()
    {
        createTopicStorage = new Mock<ICreateTopicStorage>();

        forumExistsSetup = createTopicStorage.Setup(t => t.ForumExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        createTopicSetup = createTopicStorage.Setup(t => t.CreateTopic(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();

        identityProvider.Setup(i => i.Current).Returns(identity.Object);
        getCurrentUserIdSetup = identity.Setup(i => i.UserId);

        intentionManager = new Mock<IIntentionManager>();
        intentionIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));

        var createTopicCommandValidator = new Mock<IValidator<CreateTopicCommand>>();
        createTopicCommandValidatorSetup = createTopicCommandValidator.Setup(c => c.Validate(new CreateTopicCommand(It.IsAny<Guid>(), It.IsAny<string>())));

        sut = new CreateTopicUseCase(createTopicCommandValidator.Object, identityProvider.Object, intentionManager.Object, createTopicStorage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        createTopicCommandValidatorSetup.Returns(new ValidationResult());  // it means validation successful

        var forumId = Guid.Parse("{179A7700-0960-4305-84EE-A5682CE5A96E}");
        var createTopicCommand = new CreateTopicCommand(forumId, "New Topic");


        intentionIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.Execute(createTopicCommand, It.IsAny<CancellationToken>()))
            .Should().ThrowAsync<IntentionManagerException>();

        intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
    }

    [Fact]
    public async Task Throw_ForumNotFoundException_WhenMatchingForumNotFound()
    {
        var forumId = Guid.Parse("6D44540F-FC9D-46BB-88ED-40A393EAD848");
        var createTopicCommand = new CreateTopicCommand(forumId, "New Topic");

        createTopicCommandValidatorSetup.Returns(new ValidationResult());

        intentionIsAllowedSetup.Returns(true);
        forumExistsSetup.ReturnsAsync(false);

        await sut.Invoking(s => s.Execute(createTopicCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<ForumNotFoundException>();

        createTopicStorage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("{571FA6A6-0BE0-4AB2-B5D1-63C0A26C585B}");
        var authorId = Guid.Parse("{9571CAD3-45A5-4C79-9F29-9FD5DFC3F28D}");
        var createTopicCommand = new CreateTopicCommand(forumId, "Hello world");

        var expected = new Models.Topic();

        createTopicCommandValidatorSetup.Returns(new ValidationResult());

        intentionIsAllowedSetup.Returns(true);
        forumExistsSetup.ReturnsAsync(true);

        createTopicSetup.ReturnsAsync(expected);
        getCurrentUserIdSetup.Returns(authorId);


        var actual = await sut.Execute(createTopicCommand, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
        createTopicStorage.Verify(t => t.CreateTopic(forumId, "Hello world", authorId, It.IsAny<CancellationToken>()), Times.Once);
    }
}