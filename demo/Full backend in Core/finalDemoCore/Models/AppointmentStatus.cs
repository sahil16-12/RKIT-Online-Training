namespace backend.Models
{
    /// <summary>
    /// Represents appointment lifecycle states.
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// Represents an appointment waiting for doctor action.
        /// </summary>
        PENDING,

        /// <summary>
        /// Represents an appointment approved by doctor.
        /// </summary>
        APPROVED,

        /// <summary>
        /// Represents an appointment declined by doctor.
        /// </summary>
        DECLINED,

        /// <summary>
        /// Represents an appointment cancelled by doctor.
        /// </summary>
        CANCELLED
    }
}
