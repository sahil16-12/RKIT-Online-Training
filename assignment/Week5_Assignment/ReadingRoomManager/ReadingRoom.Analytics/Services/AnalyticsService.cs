using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadingRoom.Api.Models;
using ReadingRoom.Analytics.Data;

namespace ReadingRoom.Analytics.Services
{
    /// <summary>
    /// Provides LINQ-based analytics on rooms and reservations.
    /// Demonstrates List<T> and DataTable queries.
    /// </summary>
    public class AnalyticsService
    {
        private readonly List<Room> rooms;
        private readonly List<Reservation> reservations;
        private readonly HttpClient http;

        /// <summary>
        /// Create a new AnalyticsService. Attempts to load rooms and reservations from the API at <paramref name="apiBaseUrl"/>.
        /// If the API is unreachable or returns an error, falls back to in-memory SampleData.
        /// </summary>
        /// <param name="apiBaseUrl">Base URL of the ReadingRoom API (e.g. http://localhost:5000)</param>
        public AnalyticsService(string apiBaseUrl = "http://localhost:5000")
        {
            rooms = new List<Room>();
            reservations = new List<Reservation>();

            http = new HttpClient();
            http.BaseAddress = new Uri(apiBaseUrl);

           
            try
            {
                Task.Run(async () => await LoadDataFromApi()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: could not load data from API at {apiBaseUrl}. Falling back to sample data. Error: {ex.Message}");
                var sampleRooms = SampleData.GetRooms();
                var sampleReservations = SampleData.GetReservations();
                rooms.AddRange(sampleRooms);
                reservations.AddRange(sampleReservations);
            }
        }

        private async Task LoadDataFromApi()
        {
            // Get rooms
            List<Room> apiRooms = await http.GetFromJsonAsync<List<Room>>("/rooms");
            if (apiRooms != null)
            {
                rooms.AddRange(apiRooms);
            }

            // For reservations the API exposes a room-specific endpoint with date range parameters.
            // We'll fetch reservations for each room using a wide date range to get existing entries.
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = new DateTime(2100, 1, 1);

            foreach (Room room in rooms)
            {
                // Build query string: /reservations?roomId=1&from=2025-01-01T00:00:00&to=2026-01-01T00:00:00
                string request = $"/reservations?roomId={room.Id}&from={Uri.EscapeDataString(from.ToString("o"))}&to={Uri.EscapeDataString(to.ToString("o"))}";
                var roomReservations = await http.GetFromJsonAsync<List<Reservation>>(request);
                if (roomReservations != null)
                {
                    reservations.AddRange(roomReservations);
                }
            }
        }

        /// <summary>
        /// Returns top N busiest rooms within a date range using List<T>.
        /// </summary>
        public List<(string RoomName, int Count)> GetBusiestRoomsList(DateTime from, DateTime to, int topN)
        {
            var query = reservations
                .Where(r => r.Start >= from && r.End <= to)
                .GroupBy(r => r.RoomId)
                .Select(g => new
                {
                    RoomId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(topN)
                .Join(rooms, a => a.RoomId, b => b.Id, (a, b) => (b.Name, a.Count))
                .ToList();

            return query;
        }

        /// <summary>
        /// Finds reservations that conflict (overlap in time) within a room.
        /// </summary>
        public List<(int RoomId, string Patron1, string Patron2)> GetConflictsList()
        {
            List<(int, string, string)> conflicts = new List<(int, string, string)>();

            foreach (IGrouping<int, Reservation> roomGroup in reservations.GroupBy(r => r.RoomId))
            {
                List<Reservation> list = roomGroup.ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        if (list[i].Start < list[j].End && list[j].Start < list[i].End)
                        {
                            conflicts.Add((roomGroup.Key, list[i].PatronName, list[j].PatronName));
                        }
                    }
                }
            }

            return conflicts;
        }

        /// <summary>
        /// Calculates room utilization percentage (total booked hours / total possible hours).
        /// </summary>
        public List<(string RoomName, double Utilization)> GetUtilizationList()
        {
            double totalPossibleHours = 8; // assume 8-hour day

            var result = reservations
                .GroupBy(r => r.RoomId)
                .Select(g => new
                {
                    RoomId = g.Key,
                    TotalHours = g.Sum(r => (r.End - r.Start).TotalHours)
                })
                .Join(rooms, a => a.RoomId, b => b.Id, (a, b) => (b.Name, (a.TotalHours / totalPossibleHours) * 100))
                .ToList();

            return result;
        }

        /// <summary>
        /// Creates a DataTable version of the same data for comparison.
        /// </summary>
        public DataTable CreateReservationTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("RoomId", typeof(int));
            table.Columns.Add("PatronName", typeof(string));
            table.Columns.Add("Start", typeof(DateTime));
            table.Columns.Add("End", typeof(DateTime));

            foreach (var r in reservations)
            {
                table.Rows.Add(r.Id, r.RoomId, r.PatronName, r.Start, r.End);
            }

            return table;
        }

        /// <summary>
        /// Demonstrates a simple LINQ query on a DataTable.
        /// </summary>
        public void GetBusiestRoomsDataTable()
        {
            DataTable table = CreateReservationTable();

            var query = from row in table.AsEnumerable()
                        group row by row.Field<int>("RoomId") into g
                        select new
                        {
                            RoomId = g.Key,
                            Count = g.Count()
                        };

            foreach (var result in query)
            {
                Console.WriteLine($"Room ID: {result.RoomId}, Reservations: {result.Count}");
            }
        }
    }
}
