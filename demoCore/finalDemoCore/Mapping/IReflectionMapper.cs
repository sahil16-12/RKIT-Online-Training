namespace backend.Mapping
{
    /// <summary>
    /// Defines reflection-based object mapping operations.
    /// </summary>
    public interface IReflectionMapper
    {
        #region Public Methods

        /// <summary>
        /// Maps source instance into a new destination instance.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="source">The source instance.</param>
        /// <returns>A mapped destination instance.</returns>
        TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new();

        /// <summary>
        /// Maps source instance values into an existing destination instance.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="source">The source instance.</param>
        /// <param name="destination">The destination instance.</param>
        void MapToExisting<TSource, TDestination>(TSource source, TDestination destination);

        #endregion
    }
}
