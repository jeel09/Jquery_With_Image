using ManageProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageProfile.DataAccess.Repository.IRepository
{
    public interface IStateRepository : IRepository<State>
    {
        void Update(State obj);

        IEnumerable<State> GetAllStates();

        void AddState(State state);

        void UpdateState(State state);
        void DeleteState(State state);
    }
}
