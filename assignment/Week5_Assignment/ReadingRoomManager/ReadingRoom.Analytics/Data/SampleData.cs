using System;
using System.Collections.Generic;
using ReadingRoom.Api.Models;

namespace ReadingRoom.Analytics.Data
{
    /// <summary>
    /// Provides sample in-memory data for analytics.
    /// </summary>
    public static class SampleData
    {
        public static List<Room> GetRooms()
        {
            return new List<Room>
            {
                new Room { Id = 1, Name = "Hall A", Capacity = 20 },
                new Room { Id = 2, Name = "Hall B", Capacity = 15 },
                new Room { Id = 3, Name = "Hall C", Capacity = 25 }
            };
        }

        public static List<Reservation> GetReservations()
        {
            return new List<Reservation>
            {
                new Reservation { Id = 1, RoomId = 1, PatronName = "Sahil", Start = DateTime.Parse("2025-10-01 10:00"), End = DateTime.Parse("2025-10-01 12:00"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 2, RoomId = 1, PatronName = "Danish", Start = DateTime.Parse("2025-10-02 11:00"), End = DateTime.Parse("2025-10-02 14:00"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 3, RoomId = 2, PatronName = "Hakim", Start = DateTime.Parse("2025-10-03 09:00"), End = DateTime.Parse("2025-10-03 11:00"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 4, RoomId = 3, PatronName = "Sameed", Start = DateTime.Parse("2025-10-04 10:00"), End = DateTime.Parse("2025-10-04 13:00"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 5, RoomId = 3, PatronName = "Nahin", Start = DateTime.Parse("2025-10-05 14:00"), End = DateTime.Parse("2025-10-05 17:00"), Status = ReservationStatus.Confirmed }
            };
        }
    }
}
