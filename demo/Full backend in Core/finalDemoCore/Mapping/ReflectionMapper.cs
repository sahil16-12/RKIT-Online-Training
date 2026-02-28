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

            PropertyInfo[] sourceProperties = sourceType.GetProperties(); // Will return private and instance properties of object
            PropertyInfo[] destinationProperties = destinationType.GetProperties();

            Dictionary<string, PropertyInfo> sourceByName = sourceProperties
                .ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

            Dictionary<string, PropertyInfo> destinationByName = destinationProperties
                .Where(property => property.CanWrite)
                .ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

            //Destination driven mapping
            foreach (PropertyInfo destinationProperty in destinationProperties.Where(property => property.CanWrite))
            {
                string sourceName = destinationProperty.GetCustomAttribute<MapPropertyAttribute>()?.PropertyName
                    ?? destinationProperty.Name;

                if (sourceByName.TryGetValue(sourceName, out PropertyInfo? sourceProperty) && sourceProperty.CanRead)
                {
                    object? sourceValue = sourceProperty.GetValue(source);
                    if (TryConvertValue(sourceValue, destinationProperty.PropertyType, out object? convertedValue))
                    {
                        destinationProperty.SetValue(destination, convertedValue);
                    }
                }
            }

            //Source driven mapping 
            foreach (PropertyInfo sourceProperty in sourceProperties.Where(property => property.CanRead))
            {
                string? destinationName = sourceProperty.GetCustomAttribute<MapPropertyAttribute>()?.PropertyName;

                if (string.IsNullOrWhiteSpace(destinationName))
                {
                    continue;
                }

                if (destinationByName.TryGetValue(destinationName, out PropertyInfo? destinationProperty))
                {
                    object? sourceValue = sourceProperty.GetValue(source);
                    if (TryConvertValue(sourceValue, destinationProperty.PropertyType, out object? convertedValue))
                    {
                        destinationProperty.SetValue(destination, convertedValue);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Attempts to convert source value into destination type.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="destinationType">The destination property type.</param>
        /// <param name="convertedValue">The converted output value.</param>
        /// <returns>True when conversion succeeds; otherwise false.</returns>
        private bool TryConvertValue(object? value, Type destinationType, out object? convertedValue)
        {
            convertedValue = null;

            if (value == null)
            {
                if (!destinationType.IsValueType || Nullable.GetUnderlyingType(destinationType) != null)
                {
                    return true;
                }

                return false;
            }

            Type targetType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

            if (targetType.IsAssignableFrom(value.GetType()))
            {
                convertedValue = value;
                return true;
            }

            try
            {
                if (targetType.IsEnum)
                {
                    if (value is string enumText)
                    {
                        convertedValue = Enum.Parse(targetType, enumText, true);
                        return true;
                    }

                    convertedValue = Enum.ToObject(targetType, value);
                    return true;
                }

                convertedValue = Convert.ChangeType(value, targetType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
