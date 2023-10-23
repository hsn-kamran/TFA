using AutoMapper;

namespace TFA.Storage.Mapping;

internal class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, TFA.Domain.Models.Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(t => t.TopicId));
    }
}
