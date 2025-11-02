using System;
using System.Data;
using System.Linq;

namespace ReadingRoom.Analytics
{
    /// <summary>
    /// Demonstrates LINQ-to-DataSet queries (DataTable version).
    /// </summary>
    public static class DataTableAnalytics
    {
        /// <summary>
        /// Finds top N busiest rooms using DataTable LINQ queries.
        /// </summary>
        public static void ShowTopBusiestRooms(DataTable rooms, DataTable reservations, int topN)
        {
            var query = from room in rooms.AsEnumerable()
                        join res in reservations.AsEnumerable()
                        on room.Field<int>("Id") equals res.Field<int>("RoomId")
                        where res.Field<string>("Status") == "Confirmed"
                        group res by room.Field<string>("Name") into g
                        orderby g.Count() descending
                        select new { RoomName = g.Key, Count = g.Count() };

            Console.WriteLine("Top busiest rooms (DataTable version):");
            foreach (var item in query.Take(topN))
            {
                Console.WriteLine($"{item.RoomName} — {item.Count} reservations");
            }
        }

        /// <summary>
        /// Calculates room utilization percentage using DataTables.
        /// </summary>
        public static void CalculateRoomUtilization(DataTable rooms, DataTable reservations)
        {
            var query = from room in rooms.AsEnumerable()
                        join res in reservations.AsEnumerable()
                        on room.Field<int>("Id") equals res.Field<int>("RoomId") into resGroup
                        select new
                        {
                            RoomName = room.Field<string>("Name"),
                            Utilization = resGroup.Count(r => r.Field<string>("Status") == "Confirmed") * 10.0
                        };

            Console.WriteLine("\nRoom Utilization % (DataTable version):");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.RoomName}: {item.Utilization}% utilized");
            }
        }
    }
}
