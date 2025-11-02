using ReadingRoom.Api.Models;
using ReadingRoom.Analytics;
using System.Data;


//// HAVE TO CONNECT THIS WITH REAL DATABASE WHICH IS ALREADY THERE IN PROMPT

Console.WriteLine("==== Reading Room Analytics ====\n");

// Sample data for rooms
List<Room> rooms = new List<Room>
            {
                new Room { Id = 1, Name = "Room A", Capacity = 10 },
                new Room { Id = 2, Name = "Room B", Capacity = 20 },
                new Room { Id = 3, Name = "Room C", Capacity = 15 }
            };

// Sample data for reservations
List<Reservation> reservations = new List<Reservation>
            {
                new Reservation { Id = 1, RoomId = 1, PatronName = "Alice", Start = DateTime.Parse("2025-10-01"), End = DateTime.Parse("2025-10-01"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 2, RoomId = 2, PatronName = "Bob", Start = DateTime.Parse("2025-10-01"), End = DateTime.Parse("2025-10-01"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 3, RoomId = 1, PatronName = "Charlie", Start = DateTime.Parse("2025-10-02"), End = DateTime.Parse("2025-10-02"), Status = ReservationStatus.Confirmed },
                new Reservation { Id = 4, RoomId = 3, PatronName = "David", Start = DateTime.Parse("2025-10-03"), End = DateTime.Parse("2025-10-03"), Status = ReservationStatus.Cancelled },
                new Reservation { Id = 5, RoomId = 2, PatronName = "Eva", Start = DateTime.Parse("2025-10-04"), End = DateTime.Parse("2025-10-04"), Status = ReservationStatus.Confirmed }
            };

// Execute analytics using List<T>
RoomAnalytics analytics = new RoomAnalytics();
analytics.ShowTopBusiestRooms(rooms, reservations, 2);
analytics.FindConflictingReservations(reservations);
analytics.CalculateRoomUtilization(rooms, reservations);

// Execute analytics using DataTable
Console.WriteLine("\n==== DataTable Version ====\n");
DataTable roomTable = DataSeeder.CreateRoomTable();
DataTable reservationTable = DataSeeder.CreateReservationTable();
DataTableAnalytics.ShowTopBusiestRooms(roomTable, reservationTable, 2);
DataTableAnalytics.CalculateRoomUtilization(roomTable, reservationTable);

Console.WriteLine("\n==== End of Analytics ====");
