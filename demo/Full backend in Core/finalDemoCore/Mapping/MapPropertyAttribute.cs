namespace backend.Mapping
{
    /// <summary>
    /// Defines mapped counterpart property name for reflection-based mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MapPropertyAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The counterpart property name.</param>
        public MapPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the counterpart property name.
        /// </summary>
        public string PropertyName { get; }

        #endregion
    }
}