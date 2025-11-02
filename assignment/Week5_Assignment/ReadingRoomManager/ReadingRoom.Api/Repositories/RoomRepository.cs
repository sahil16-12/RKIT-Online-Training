using ReadingRoom.Api.Models;
using ReadingRoom.Api.Data;
using System.Data;
using ServiceStack.OrmLite;

namespace ReadingRoom.Api.Repositories
{
    /// <summary>
    /// Handles data operations for Room entity.
    /// </summary>
    public class RoomRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public RoomRepository(DbConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public List<Room> GetAll()
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                return db.Select<Room>();
            }
        }

        public Room GetById(int id)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                return db.SingleById<Room>(id);
            }
        }

        public void Add(Room room)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.Insert(room);
            }
        }

        public void Update(Room room)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.Update(room);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = _connectionFactory.Open())
            {
                db.DeleteById<Room>(id);
            }
        }
    }
}
