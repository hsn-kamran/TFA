namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumsStorage forumsStorage;

    public GetForumsUseCase(IGetForumsStorage forumsStorage)
    {
        this.forumsStorage = forumsStorage;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
    {
        return await forumsStorage.GetForums(cancellationToken);
    }
}
