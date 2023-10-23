using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Storage;

internal class CreateTopicStorage : ICreateTopicStorage
{
    private readonly IGuidFactory guidFactory;
    private readonly IMomentProvider momentProvider;
    private readonly IMapper mapper;
    private readonly ForumDbContext context;

    public CreateTopicStorage(IGuidFactory guidFactory, IMomentProvider momentProvider, IMapper mapper, ForumDbContext context)
    {
        this.guidFactory = guidFactory;
        this.momentProvider = momentProvider;
        this.mapper = mapper;
        this.context = context;
    }

    public Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken)
        => context.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);

    public async Task<Domain.Models.Topic> CreateTopic(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();

        await context.AddAsync(new TFA.Storage.Topic
        {
            TopicId = topicId,
            Title = title,
            ForumId = forumId,
            UserId = authorId,
            CreatedAt = momentProvider.Now
        });

        await context.SaveChangesAsync();


        return await context.Topics.Where(t => t.TopicId == topicId)
                .ProjectTo<Domain.Models.Topic>(mapper.ConfigurationProvider)
                .SingleAsync();
    }
}
