namespace TFA.Domain.Authorization;

internal interface IIntentionManager
{
    bool IsAllowed<IIntention>(IIntention intention) where IIntention : struct; 
}
