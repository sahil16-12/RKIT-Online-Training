using ServiceStack.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Represents user master data table.
    /// </summary>
    [Alias("users")]
    public class TBL01
    {
        #region Public Properties

        /// <summary>
        /// Represents user identifier.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public int L01F01 { get; set; }

        /// <summary>
        /// Represents user role type.
        /// </summary>
        [Alias("user_type")]
        public UserType L01F02 { get; set; }

        /// <summary>
        /// Represents user's full name.
        /// </summary>
        [Alias("full_name")]
        public string L01F03 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's email address.
        /// </summary>
        [Index(Unique = true)]
        [Alias("email")]
        public string L01F04 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's phone number.
        /// </summary>
        [Alias("phone")]
        public string L01F05 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's date of birth.
        /// </summary>
        [Alias("dob")]
        public DateTime L01F06 { get; set; }

        /// <summary>
        /// Represents user's password hash.
        /// </summary>
        [Alias("password")]
        public string L01F07 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user creation UTC date and time.
        /// </summary>
        [Alias("created_at")]
        public DateTime L01F08 { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Represents linked patient information.
        /// </summary>
        [Ignore]
        public TBL02? L01F09 { get; set; }

        /// <summary>
        /// Represents linked doctor information.
        /// </summary>
        [Ignore]
        public TBL03? L01F10 { get; set; }

        #endregion
    }
}
