using Application;
using AutoMapper;

namespace InfraStructrue.Mappers.AutoMapper
{
    public class AutoMapperCustom : ICustomMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperCustom(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSoure, TDestination>(TSoure source)
        {
            return _mapper.Map<TSoure, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
