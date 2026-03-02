using ServiceStack.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Represents doctor detail data table.
    /// </summary>
    [Alias("doctors")]
    public class TBL03
    {
        #region Public Properties

        /// <summary>
        /// Represents doctor row identifier.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public int L03F01 { get; set; }

        /// <summary>
        /// Represents related user identifier.
        /// </summary>
        [References(typeof(TBL01))]
        [ForeignKey(typeof(TBL01), OnDelete = "CASCADE")]
        [Alias("user_id")]
        public int L03F02 { get; set; }

        /// <summary>
        /// Represents doctor specialization.
        /// </summary>
        [Alias("specialization")]
        public string? L03F03 { get; set; }

        /// <summary>
        /// Represents doctor license number.
        /// </summary>
        [Alias("license_number")]
        public string? L03F04 { get; set; }

        /// <summary>
        /// Represents years of doctor experience.
        /// </summary>
        [Alias("years_experience")]
        public int? L03F05 { get; set; }

        /// <summary>
        /// Represents doctor record creation UTC date and time.
        /// </summary>
        [Alias("created_at")]
        public DateTime L03F06 { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Represents related user master data.
        /// </summary>
        [Ignore]
        public TBL01 L03F07 { get; set; } = null!;

        #endregion
    }
}
