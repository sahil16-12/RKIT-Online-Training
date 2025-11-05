using ReadingRoom.Analytics.Services;

string apiBaseUrl = "http://localhost:5000";
AnalyticsService analytics = new AnalyticsService(apiBaseUrl);

Console.WriteLine("=== Busiest Rooms (List<T>) ===");
foreach (var r in analytics.GetBusiestRoomsList(DateTime.Parse("2025-10-01"), DateTime.Parse("2025-10-10"), 3))
{
    Console.WriteLine($"Room: {r.RoomName}, Reservations: {r.Count}");
}

Console.WriteLine("\n=== Conflicting Reservations ===");
foreach (var c in analytics.GetConflictsList())
{
    Console.WriteLine($"Room ID: {c.RoomId}, {c.Patron1} conflicts with {c.Patron2}");
}

Console.WriteLine("\n=== Utilization % per Room ===");
foreach (var u in analytics.GetUtilizationList())
{
    Console.WriteLine($"Room: {u.RoomName}, Utilization: {u.Utilization:F2}%");
}

Console.WriteLine("\n=== DataTable Demo ===");
analytics.GetBusiestRoomsDataTable();

Console.WriteLine("\nAnalytics completed successfully.");