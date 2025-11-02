using ServiceStack.DataAnnotations;
namespace ReadingRoom.Api.Models
{


    /// <summary>
    /// Represents a library reading room entity.
    /// </summary>
    public class Room
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Capacity { get; set; }
    }

}
