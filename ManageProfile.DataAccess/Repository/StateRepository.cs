using ManageProfile.DataAccess.Data;
using ManageProfile.DataAccess.Repository.IRepository;
using ManageProfile.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.DataAccess.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly ManageProfileContext _db;
        public StateRepository(ManageProfileContext db) : base(db)
        {
            _db = db;
        }

        public void Update(State obj)
        {
            _db.States.Update(obj);
        }

        public IEnumerable<State> GetAllStates()
        {
            var allStates = _db.States.FromSqlRaw("EXECUTE SelectAllState").ToList();
            foreach (var state in allStates)
            {
                _db.Entry(state).Reference(s => s.Country).Load();
            }
            return allStates;
        }

        public void AddState(State state)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE InsertState {0}, {1}", state.StateName, state.CountryId);
        }

        public void UpdateState(State state)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE UpdateState {0}, {1}, {2}", state.StateId, state.StateName, state.CountryId);
        }

        public void DeleteState(State state)
        {
            _db.Database.ExecuteSqlRaw("EXECUTE DeleteState {0}", state.StateId);
        }
    }
}
