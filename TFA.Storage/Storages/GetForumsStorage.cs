using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage;

internal class GetForumsStorage : IGetForumsStorage
{
    private readonly IMemoryCache memoryCache;
    private readonly IMapper mapper;
    private readonly ForumDbContext forumDbContext;

    public GetForumsStorage(IMemoryCache memoryCache, IMapper mapper, ForumDbContext forumDbContext)
    {
        this.memoryCache = memoryCache;
        this.mapper = mapper;
        this.forumDbContext = forumDbContext;
    }

    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync<Domain.Models.Forum[]>(nameof(GetForums), factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return forumDbContext
                    .Forums
                    .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
        });
    }
}
