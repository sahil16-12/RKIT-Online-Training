using ServiceStack.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Represents patient detail data table.
    /// </summary>
    [Alias("patients")]
    public class TBL02
    {
        #region Public Properties

        /// <summary>
        /// Represents patient row identifier.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public int L02F01 { get; set; }

        /// <summary>
        /// Represents related user identifier.
        /// </summary>
        [References(typeof(TBL01))]
        [ForeignKey(typeof(TBL01), OnDelete = "CASCADE")]
        [Alias("user_id")]
        public int L02F02 { get; set; }

        /// <summary>
        /// Represents emergency contact number.
        /// </summary>
        [Alias("emergency_contact")]
        public string? L02F03 { get; set; }

        /// <summary>
        /// Represents patient allergy notes.
        /// </summary>
        [Alias("allergies")]
        public string? L02F04 { get; set; }

        /// <summary>
        /// Represents patient record creation UTC date and time.
        /// </summary>
        [Alias("created_at")]
        public DateTime L02F05 { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Represents related user master data.
        /// </summary>
        [Ignore]
        public TBL01 L02F06 { get; set; } = null!;

        #endregion
    }
}
