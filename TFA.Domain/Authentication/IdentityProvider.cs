namespace TFA.Domain.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => new IdentityUser(Guid.Parse("{7F09E25C-B680-4592-86C1-8E6C95331405}"));
}
