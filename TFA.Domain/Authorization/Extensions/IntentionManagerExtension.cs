using TFA.Domain.Exceptions;

namespace TFA.Domain.Authorization.Extensions;

internal static class IntentionManagerExtension
{
    public static void ThrowIfForbidden<IIntention>(this IIntentionManager manager, IIntention intention)
        where IIntention : struct
    {
        if (!manager.IsAllowed(intention))
            throw new IntentionManagerException();
    }
}
