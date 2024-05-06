using ManageProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.DataAccess.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void Update(Country obj);

        IEnumerable<Country> GetAllCountry();

        void AddCountry(Country obj);

        void UpdateCountry(Country obj);

        void DeleteCountry(Country obj);
    }
}
