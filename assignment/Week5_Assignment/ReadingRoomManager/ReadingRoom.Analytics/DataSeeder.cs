using System;
using System.Data;

namespace ReadingRoom.Analytics
{
    /// <summary>
    /// Provides helper methods to create sample DataTables for analytics.
    /// </summary>
    public static class DataSeeder
    {
        public static DataTable CreateRoomTable()
        {
            DataTable table = new DataTable("Rooms");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Capacity", typeof(int));

            table.Rows.Add(1, "Room A", 10);
            table.Rows.Add(2, "Room B", 20);
            table.Rows.Add(3, "Room C", 15);

            return table;
        }

        public static DataTable CreateReservationTable()
        {
            DataTable table = new DataTable("Reservations");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("RoomId", typeof(int));
            table.Columns.Add("PatronName", typeof(string));
            table.Columns.Add("Start", typeof(DateTime));
            table.Columns.Add("End", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));

            table.Rows.Add(1, 1, "Alice", DateTime.Parse("2025-10-01"), DateTime.Parse("2025-10-01"), "Confirmed");
            table.Rows.Add(2, 2, "Bob", DateTime.Parse("2025-10-01"), DateTime.Parse("2025-10-01"), "Confirmed");
            table.Rows.Add(3, 1, "Charlie", DateTime.Parse("2025-10-02"), DateTime.Parse("2025-10-02"), "Confirmed");
            table.Rows.Add(4, 3, "David", DateTime.Parse("2025-10-03"), DateTime.Parse("2025-10-03"), "Cancelled");
            table.Rows.Add(5, 2, "Eva", DateTime.Parse("2025-10-04"), DateTime.Parse("2025-10-04"), "Confirmed");

            return table;
        }
    }
}
