using ManageProfile.DataAccess.Data;
using ManageProfile.DataAccess.Repository.IRepository;
using ManageProfile.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ManageProfileContext _db;
        public UserRepository(ManageProfileContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }

        public IEnumerable<User> GetAllUser()
        {
            var allUsers = _db.Users.FromSqlRaw("EXECUTE SelectAllUser").ToList();
            foreach (var user in allUsers)
            {
                _db.Entry(user).Reference(u => u.Gender).Load();
                _db.Entry(user).Reference(u => u.Hobby).Load();
                _db.Entry(user).Reference(u => u.Country).Load();
                _db.Entry(user).Reference(u => u.State).Load();
                _db.Entry(user).Reference(u => u.City).Load();
            }
            return allUsers;
        }

        public void AddUser(User user)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE InsertUser {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", user.FirstName, user.LastName, user.Address, user.GenderId, user.HobbyId, user.CountryId, user.StateId, user.CityId, user.ImageUrl);
        }

        public void UpdateUser(User user)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE UpdateUser {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", user.UserId, user.FirstName, user.LastName, user.Address, user.GenderId, user.HobbyId, user.CountryId, user.StateId, user.CityId, user.ImageUrl);
        }

        public void DeleteUser(User user)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE DeleteUser {0}", user.UserId);
        }
    }
}
