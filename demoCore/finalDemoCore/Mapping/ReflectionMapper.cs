using System.Reflection;

namespace backend.Mapping
{
    /// <summary>
    /// Provides reflection-based mapping between DTO and POCO types.
    /// </summary>
    public class ReflectionMapper : IReflectionMapper
    {
        #region Public Methods

        /// <inheritdoc/>
        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            TDestination destination = new TDestination();
            MapToExisting(source, destination);
            return destination;
        }

        /// <inheritdoc/>
        public void MapToExisting<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] destinationProperties = destinationType.GetProperties();

            // Build lookup for source properties by name
            Dictionary<string, PropertyInfo> sourceByName = sourceProperties
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            // Map properties by matching names
            foreach (PropertyInfo destinationProperty in destinationProperties.Where(p => p.CanWrite))
            {
                if (sourceByName.TryGetValue(destinationProperty.Name, out PropertyInfo? sourceProperty))
                {
                    object sourceValue = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, sourceValue);
                }
            }
        }

        #endregion
    }
}
