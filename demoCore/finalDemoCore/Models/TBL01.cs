using ServiceStack.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Represents user master data table.
    /// </summary>

    public class TBL01
    {
        #region Public Properties

        /// <summary>
        /// Represents user identifier.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int L01F01 { get; set; }

        /// <summary>
        /// Represents user role type.
        /// </summary>

        public UserType L01F02 { get; set; }

        /// <summary>
        /// Represents user's full name.
        /// </summary>

        public string L01F03 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's email address.
        /// </summary>
        [Index(Unique = true)]

        public string L01F04 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's phone number.
        /// </summary>

        public string L01F05 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user's date of birth.
        /// </summary>

        public DateTime L01F06 { get; set; }

        /// <summary>
        /// Represents user's password hash.
        /// </summary>

        public string L01F07 { get; set; } = string.Empty;

        /// <summary>
        /// Represents user creation UTC date and time.
        /// </summary>

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
