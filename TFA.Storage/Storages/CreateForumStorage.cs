using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.CreateForum;

namespace TFA.Storage;

internal class CreateForumStorage : ICreateForumStorage
{
    private readonly IGuidFactory guidFactory;
    private readonly IMapper mapper;
    private readonly ForumDbContext context;

    public CreateForumStorage(IGuidFactory guidFactory, IMapper mapper, ForumDbContext context)
    {
        this.guidFactory = guidFactory;
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<Domain.Models.Forum?> Create(string title, CancellationToken cancellationToken)
    {
        var forumId = guidFactory.Create();

        await context.AddAsync(new Forum
        {
            ForumId = forumId,
            Title = title,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return await context.Forums
                        .Where(f => f.ForumId == forumId)
                        .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
                        .FirstAsync(cancellationToken);
    }
}
