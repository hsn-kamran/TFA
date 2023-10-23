using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddForumDomain(this IServiceCollection services)
    {
        return services
                .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
                .AddScoped<IIdentityProvider, IdentityProvider>()
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>()
                .AddScoped<IIntentionResolver, ForumIntentionResolver>()
                .AddValidatorsFromAssemblyContaining<TFA.Domain.Models.Forum>(includeInternalTypes: true);
    }

}