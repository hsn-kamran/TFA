using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public interface IIntentionResolver
{
}


public interface IIntentionResolver<IIntention> : IIntentionResolver 
    where IIntention : struct
{
    bool IsAllowed(IIdentity subject, IIntention intention);
}
