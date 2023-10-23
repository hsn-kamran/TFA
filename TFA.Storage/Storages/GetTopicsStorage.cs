using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Storage;

internal class GetTopicsStorage : IGetTopicsStorage
{
    private readonly IMapper mapper;
    private readonly ForumDbContext dbContext;

    public GetTopicsStorage(IMapper mapper, ForumDbContext dbContext)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
    }


    public async Task<(IEnumerable<Domain.Models.Topic>, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics.Where(t => t.ForumId == forumId);
        int totalCount = await query.CountAsync(cancellationToken);

        var topics =  await query.Skip(skip)
                                 .Take(take)
                                 .ProjectTo<Domain.Models.Topic>(mapper.ConfigurationProvider)
                                 .ToListAsync();

        return (topics, totalCount);
    }
}
