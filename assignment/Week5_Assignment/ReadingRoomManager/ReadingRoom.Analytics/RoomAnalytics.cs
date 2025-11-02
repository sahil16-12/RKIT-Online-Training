using System;
using System.Collections.Generic;
using System.Linq;
using ReadingRoom.Api.Models;

namespace ReadingRoom.Analytics
{
    /// <summary>
    /// Performs analytical LINQ queries using in-memory List<T> collections.
    /// </summary>
    public class RoomAnalytics
    {
        /// <summary>
        /// Displays top N busiest rooms based on total number of reservations.
        /// </summary>
        public void ShowTopBusiestRooms(List<Room> rooms, List<Reservation> reservations, int topN)
        {
            Console.WriteLine("Top busiest rooms (List version):");

            List<(string RoomName, int Count)> topRooms = (from room in rooms
                                                           join res in reservations on room.Id equals res.RoomId
                                                           where res.Status == ReservationStatus.Confirmed
                                                           group res by room.Name into roomGroup
                                                           orderby roomGroup.Count() descending
                                                           select (RoomName: roomGroup.Key, Count: roomGroup.Count()))
                                                           .Take(topN)
                                                           .ToList();

            foreach ((string RoomName, int Count) room in topRooms)
            {
                Console.WriteLine($"{room.RoomName} — {room.Count} reservations");
            }
        }

        /// <summary>
        /// Finds overlapping/conflicting reservations for the same room.
        /// </summary>
        public void FindConflictingReservations(List<Reservation> reservations)
        {
            Console.WriteLine("\nConflicting Reservations:");

            List<(int RoomId, string Patron1, string Patron2)> conflicts = (from r1 in reservations
                                                                            from r2 in reservations
                                                                            where r1.RoomId == r2.RoomId &&
                                                                                  r1.Id < r2.Id &&
                                                                                  r1.Start == r2.Start
                                                                            select (r1.RoomId, r1.PatronName, r2.PatronName))
                                                                            .ToList();

            foreach ((int RoomId, string Patron1, string Patron2) conflict in conflicts)
            {
                Console.WriteLine($"Room {conflict.RoomId}: {conflict.Patron1} conflicts with {conflict.Patron2}");
            }
        }

        /// <summary>
        /// Calculates room utilization percentage based on number of confirmed reservations.
        /// </summary>
        public void CalculateRoomUtilization(List<Room> rooms, List<Reservation> reservations)
        {
            Console.WriteLine("\nRoom Utilization % (List version):");

            List<(string RoomName, double Utilization)> utilizationData = (from room in rooms
                                                                           join res in reservations on room.Id equals res.RoomId into resGroup
                                                                           let confirmedCount = resGroup.Count(r => r.Status == ReservationStatus.Confirmed)
                                                                           select (RoomName: room.Name, Utilization: confirmedCount * 10.0))
                                                                           .ToList();

            foreach ((string RoomName, double Utilization) item in utilizationData)
            {
                Console.WriteLine($"{item.RoomName}: {item.Utilization}% utilized");
            }
        }
    }
}
