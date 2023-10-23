using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public class IntentionManager : IIntentionManager
{
    private readonly IIdentityProvider _provider;
    private readonly IEnumerable<IIntentionResolver> _resolvers;

    public IntentionManager(IIdentityProvider provider, IEnumerable<IIntentionResolver> resolvers)
    {
        this._provider = provider;
        _resolvers = resolvers;
    }

    public bool IsAllowed<IIntention>(IIntention intention) 
        where IIntention : struct
    {
        var matchingResolver = _resolvers.OfType<IIntentionResolver<IIntention>>().FirstOrDefault();

        if (matchingResolver is null)
            return false;

        return matchingResolver.IsAllowed(_provider.Current, intention);
    }
}
