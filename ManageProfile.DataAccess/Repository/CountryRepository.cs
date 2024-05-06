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
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ManageProfileContext _db;
        public CountryRepository(ManageProfileContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Country obj)
        {
            _db.Countries.Update(obj);
        }

        public IEnumerable<Country> GetAllCountry()
        {
            return _db.Countries.FromSqlRaw("EXEC SelectAllCountry").ToList();
        }

        public void AddCountry(Country obj)
        {
            _db.Database.ExecuteSqlRaw("EXEC InsertCountry {0}", obj.CountryName);
        }

        public void UpdateCountry(Country obj)
        {
            _db.Database.ExecuteSqlRaw("EXEC UpdateCountry {0}, {1}", obj.CountryId, obj.CountryName);
        }

        public void DeleteCountry(Country obj)
        {
            _db.Database.ExecuteSqlRaw("EXEC DeleteCountry {0}", obj.CountryId);
        }
    }
}
