namespace TFA.Domain.Authentication.Extensions;

public static class IdentityExtension
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}