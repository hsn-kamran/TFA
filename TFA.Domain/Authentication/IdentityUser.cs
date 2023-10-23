namespace TFA.Domain.Authentication;

internal class IdentityUser : IIdentity
{
    public IdentityUser(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
