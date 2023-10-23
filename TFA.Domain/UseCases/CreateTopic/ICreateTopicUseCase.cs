namespace TFA.Domain.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(CreateTopicCommand createTopicCommand, CancellationToken cancellation);
}
