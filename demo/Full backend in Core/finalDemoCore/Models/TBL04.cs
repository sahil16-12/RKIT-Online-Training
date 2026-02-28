using ServiceStack.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Represents appointment booking data table.
    /// </summary>
    [Alias("appointments")]
    [CompositeIndex("doctor_user_id", "appointment_at_utc")]
    [CompositeIndex("patient_user_id", "appointment_at_utc")]
    public class TBL04
    {
        #region Public Properties

        /// <summary>
        /// Represents appointment identifier.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public int L04F01 { get; set; }

        /// <summary>
        /// Represents patient user identifier.
        /// </summary>
        [References(typeof(TBL01))]
        [ForeignKey(typeof(TBL01), OnDelete = "CASCADE")]
        [Alias("patient_user_id")]
        public int L04F02 { get; set; }

        /// <summary>
        /// Represents doctor user identifier.
        /// </summary>
        [References(typeof(TBL01))]
        [ForeignKey(typeof(TBL01), OnDelete = "CASCADE")]
        [Alias("doctor_user_id")]
        public int L04F03 { get; set; }

        /// <summary>
        /// Represents scheduled appointment UTC date and time.
        /// </summary>
        [Alias("appointment_at_utc")]
        public DateTime L04F04 { get; set; }

        /// <summary>
        /// Represents appointment reason provided by patient.
        /// </summary>
        [Alias("reason")]
        public string L04F05 { get; set; } = string.Empty;

        /// <summary>
        /// Represents appointment status.
        /// </summary>
        [Alias("status")]
        public AppointmentStatus L04F06 { get; set; } = AppointmentStatus.PENDING;

        /// <summary>
        /// Represents doctor action notes.
        /// </summary>
        [Alias("doctor_notes")]
        public string? L04F07 { get; set; }

        /// <summary>
        /// Represents appointment creation UTC date and time.
        /// </summary>
        [Alias("created_at")]
        public DateTime L04F08 { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Represents appointment last update UTC date and time.
        /// </summary>
        [Alias("updated_at")]
        public DateTime L04F09 { get; set; } = DateTime.UtcNow;

        #endregion
    }
}
