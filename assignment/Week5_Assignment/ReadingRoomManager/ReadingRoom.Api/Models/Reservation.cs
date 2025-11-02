
using ServiceStack.DataAnnotations;
using System;

namespace ReadingRoom.Api.Models
{
    /// <summary>
    /// Represents a reservation for a reading room.
    /// </summary>
    public class Reservation
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Room))]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string PatronName { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public ReservationStatus Status { get; set; }
    }

}
