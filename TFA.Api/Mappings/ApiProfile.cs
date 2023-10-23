using AutoMapper;

namespace TFA.Api.Mappings
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<TFA.Domain.Models.Forum, TFA.Api.Models.Forum>();
            CreateMap<TFA.Domain.Models.Topic, TFA.Api.Models.Topic>();
        }
    }
}