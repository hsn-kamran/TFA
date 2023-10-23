using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.UseCases.CreateForum;

internal interface ICreateForumUseCase
{
    Task Execute(CreateForumCommand command, CancellationToken cancellationToken);
}
