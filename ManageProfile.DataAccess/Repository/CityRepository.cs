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
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ManageProfileContext _db;
        public CityRepository(ManageProfileContext db) : base(db)
        {
            _db = db;
        }

        public void Update(City obj)
        {
            _db.Cities.Update(obj);
        }

        public IEnumerable<City> GetAllCity()
        {
            var allCities = _db.Cities.FromSqlRaw("EXECUTE SelectAllCity").ToList();
            foreach (var city in allCities)
            {
                _db.Entry(city).Reference(c => c.Country).Load();
                _db.Entry(city).Reference(c => c.State).Load();
            }
            return allCities;
        }

        public void AddCity(City city)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE InsertCity {0}, {1}, {2}", city.CityName, city.CountryId, city.StateId);
        }

        public void UpdateCity(City city)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE UpdateCity {0}, {1}, {2}, {3}", city.CityId, city.CityName, city.CountryId, city.StateId);
        }

        public void DeleteCity(City city)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE DeleteCity {0}", city.CityId);
        }
    }
}
