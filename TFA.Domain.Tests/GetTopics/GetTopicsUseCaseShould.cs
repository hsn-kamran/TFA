using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase sut;
    private readonly Mock<IGetTopicsStorage> storage;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic>, int totalCount)>> storageSetup;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator.Setup(q => q.Validate(It.IsAny<GetTopicsQuery>())).Returns(new ValidationResult());

        storage = new Mock<IGetTopicsStorage>();
        storageSetup = storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));
        

        sut = new GetTopicsUseCase(validator.Object, storage.Object);
    }

/*    [Fact]
    public async Task ThrowForumNotFoundException_WhenMatchedForumNotFound()
    {
        var forumId = Guid.Parse("{B66FBD81-5974-46DA-8CEE-5BB47D23027E}");
        var getTopicsQuery = new GetTopicsQuery(forumId, 0, 5);

        var actual = sut.Invoking(s => s.Execute(getTopicsQuery, It.IsAny<CancellationToken>()));

        await actual.Should().ThrowAsync<ForumNotFoundException>();
    }*/


    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage()
    {
        var forumId = Guid.Parse("{DB908144-8E5E-491C-A3F3-01547D57E225}");

        var expectedTopics = new Topic[] { new() };
        var expectedTotalCount = 6;

        storageSetup.ReturnsAsync((expectedTopics, expectedTotalCount));

        var (actualResources, actualTotalCount) = await sut.Execute(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedTopics);
        actualTotalCount.Should().Be(expectedTotalCount);

        storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}
