namespace TFA.Domain.UseCases.CreateForum;

public interface ICreateForumStorage
{
    Task<Forum?> Create(string title, CancellationToken cancellationToken);
}
