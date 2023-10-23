using FluentAssertions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests.CreateTopic;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator sut;

    public CreateTopicCommandValidatorShould()
    {
        sut = new CreateTopicCommandValidator();
    }

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var validCreateCommandCommand = new CreateTopicCommand(Guid.Parse("{6D54FE31-E233-49A2-92D8-BCDE8F535075}"), "New title");
        var validationResult = sut.Validate(validCreateCommandCommand);

        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateTopicCommands))]
    public void ReturnFailure_WhenCommandIsNotValid(CreateTopicCommand command)
    {
        var (forumId, title) = command;

        var invalidCreateCommandCommand = new CreateTopicCommand(forumId, title);
        var validationResult = sut.Validate(invalidCreateCommandCommand);

        validationResult.IsValid.Should().BeFalse();
    }


    public static IEnumerable<object[]> GetInvalidCreateTopicCommands()
    {
        var createTopicCommand = new CreateTopicCommand(Guid.Empty, null);

        yield return new object[] { createTopicCommand };
        yield return new object[] { createTopicCommand with { Title = "" } };
        yield return new object[] { createTopicCommand with { Title = "  " } };
        yield return new object[] { createTopicCommand with { Title = "title" } };

        yield return new object[] { createTopicCommand with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}") } };
        yield return new object[] { createTopicCommand with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Title = "" } };
        yield return new object[] { createTopicCommand with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Title = "  " } };
    }
}


