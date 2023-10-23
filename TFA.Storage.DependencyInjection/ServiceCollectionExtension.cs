using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
    {
        return services.AddDbContext<ForumDbContext>(options =>
                    {
                        options.UseNpgsql(dbConnectionString);
                    })
                    .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                    .AddScoped<ICreateForumStorage, CreateForumStorage>()
                    .AddScoped<IGetForumsStorage, GetForumsStorage>()
                    .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                    .AddScoped<IGuidFactory, GuidFactory>()
                    .AddScoped<IMomentProvider, MomentProvider>()
                 .AddMemoryCache()
                 .AddAutoMapper(config => config.AddMaps(Assembly.GetAssembly(typeof(ForumDbContext))));
            
    }
}