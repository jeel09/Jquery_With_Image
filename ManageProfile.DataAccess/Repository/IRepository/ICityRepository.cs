using ManageProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.DataAccess.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
        void Update(City obj);
        IEnumerable<City> GetAllCity();
        void AddCity(City city);
        void UpdateCity(City city);
        void DeleteCity(City city);
    }
}
