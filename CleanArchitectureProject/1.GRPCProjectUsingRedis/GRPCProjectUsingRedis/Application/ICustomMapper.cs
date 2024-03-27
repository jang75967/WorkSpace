namespace Application
{
    public interface ICustomMapper
    {
        //TDestination Map<TDestination>(TDestination destination);
        TDestination Map<TDestination>(object source);
        TDestination Map<TSoure, TDestination>(TSoure source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
