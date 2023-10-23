using FluentAssertions;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsQueryValidatorShould
    {
        private readonly GetTopicsQueryValidator sut;

        public GetTopicsQueryValidatorShould()
        {
            sut = new GetTopicsQueryValidator();
        }

        [Fact]
        public void ReturnSuccess_WhenQueryIsValid()
        {
            var validGetTopicsQuery = new GetTopicsQuery(Guid.Parse("{6D54FE31-E233-49A2-92D8-BCDE8F535075}"), 0, 0);
            var validationResult = sut.Validate(validGetTopicsQuery);

            validationResult.IsValid.Should().BeTrue();
        }


        [Theory]
        [MemberData(nameof(GetInvalidTopicsQueries))]
        public void ReturnFailure_WhenQueryIsNotValid(GetTopicsQuery query)
        {
            var (forumId, skip, take) = query;

            var invalidGetTopicsQuery = new GetTopicsQuery(forumId, skip, take);
            var validationResult = sut.Validate(invalidGetTopicsQuery);

            validationResult.IsValid.Should().BeFalse();
        }


        public static IEnumerable<object[]> GetInvalidTopicsQueries()
        {
            var getTopicQuery = new GetTopicsQuery(Guid.Empty, -1, -4);

            yield return new object[] { getTopicQuery };
            yield return new object[] { getTopicQuery with { Skip = -1, Take = 1 } };
            yield return new object[] { getTopicQuery with { Skip = 1, Take = -5 } };

            yield return new object[] { getTopicQuery with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Skip = 5, Take = -5 } };
            yield return new object[] { getTopicQuery with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Skip = 0, Take = -5 } };
            yield return new object[] { getTopicQuery with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Skip = -1, Take = 3 } };
            yield return new object[] { getTopicQuery with { ForumId = Guid.Parse("{64A18BA3-314D-407C-B978-F84D8C80B399}"), Skip = -1, Take = -3 } };
        }
    }
}
