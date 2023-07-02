using Application.Mappers;
using Mapster;

namespace Infrastructure.Mappers.Mapsters;

public class MapsterDI : IMapper
{
    private readonly MapsterMapper.IMapper _mapper;   
    public MapsterDI(MapsterMapper.IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
    {
        return _mapper.From(source).AdaptToType<TDestination>();
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return _mapper.From(source).AdaptToType<TDestination>();
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.From(source).AdaptToType<TDestination>();
    }
}
