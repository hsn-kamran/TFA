namespace TFA.Domain.Exceptions;

public class ForumNotFoundException : DomainException
{
    public ForumNotFoundException(Guid forumId) : base(ErrorCode.Gone, $"Forum with {forumId} was not found")
    {
        
    }
}
