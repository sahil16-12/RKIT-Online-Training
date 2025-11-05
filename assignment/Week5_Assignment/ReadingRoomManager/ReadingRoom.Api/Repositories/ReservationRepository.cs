using ReadingRoom.Api.Models;
using ReadingRoom.Api.Data;
using System.Data;
using ServiceStack.OrmLite;

namespace ReadingRoom.Api.Repositories
{
    /// <summary>
    /// Handles data operations for Reservation entity.
    /// </summary>
    public class ReservationRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ReservationRepository(DbConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public List<Reservation> GetAll()
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                return db.Select<Reservation>();
            }
        }

        public List<Reservation> GetByRoomAndDateRange(int roomId, DateTime from, DateTime to)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                return db.Select<Reservation>(x => x.RoomId == roomId && x.Start >= from && x.End <= to);
            }
        }

        public void Add(Reservation reservation)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.Insert(reservation);
            }
        }

        public void Update(Reservation reservation)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.Update(reservation);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.DeleteById<Reservation>(id);
            }
        }

    }
}
