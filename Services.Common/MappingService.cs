namespace Services.Common
{
    using Contracts;
    using Uspelite.Web.Infrastructure.Mapping;

    public class MappingService : IMappingService
    {
        public T Map<T>(object source)
        {
            var mapper = AutoMapperConfig.Configuration.CreateMapper();
            return mapper.Map<T>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var mapper = AutoMapperConfig.Configuration.CreateMapper();
            return mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}
