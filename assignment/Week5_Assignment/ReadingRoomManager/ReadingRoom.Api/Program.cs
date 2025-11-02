using Microsoft.AspNetCore.RateLimiting;
using ReadingRoom.Api.Data;
using ReadingRoom.Api.Models;
using ReadingRoom.Api.Repositories;
using ServiceStack.OrmLite;
using System.Data;
using System.Threading.RateLimiting;

// Connection string to MySQL
string connectionString = "Server=localhost;Database=readingroomdb;User ID=root;Password=sahil@1612;";

DbConnectionFactory factory = new DbConnectionFactory(connectionString);
RoomRepository roomRepo = new RoomRepository(factory);
ReservationRepository resRepo = new ReservationRepository(factory);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "fixed", options =>
{
    options.PermitLimit = 10;
    options.Window = TimeSpan.FromSeconds(30);
}));

var app = builder.Build();
app.UseRateLimiter();

// Create tables if not exist
using (IDbConnection db = factory.Open())
{
    db.CreateTableIfNotExists<Room>();
    db.CreateTableIfNotExists<Reservation>();
}

// ------------------- ROOM ENDPOINTS -------------------

app.MapGet("/rooms", () => roomRepo.GetAll());
app.MapPost("/rooms", (Room room) => { roomRepo.Add(room); return Results.Ok(); });
app.MapPut("/rooms/{id:int}", (int id, Room room) =>
{
    room.Id = id;
    roomRepo.Update(room);
    return Results.Ok();
});
app.MapDelete("/rooms/{id:int}", (int id) => { roomRepo.Delete(id); return Results.Ok(); });

// ------------------- RESERVATION ENDPOINTS -------------------

app.MapGet("/reservations", (int roomId, DateTime from, DateTime to) => resRepo.GetByRoomAndDateRange(roomId, from, to));
app.MapPost("/reservations", (Reservation r) => { resRepo.Add(r); return Results.Ok(); });
app.MapPut("/reservations/{id:int}", (int id, Reservation r) => { r.Id = id; resRepo.Update(r); return Results.Ok(); });
app.MapDelete("/reservations/{id:int}", (int id) => { resRepo.Delete(id); return Results.Ok(); });

app.Run();
