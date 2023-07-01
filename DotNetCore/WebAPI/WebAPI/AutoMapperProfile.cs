using AutoMapper;

namespace WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // get
            CreateMap<SuperHero, SuperHeroDto>();
            
            // post
            CreateMap<SuperHeroDto, SuperHero>();
        }
    }
}
