using Microsoft.AspNetCore.Mvc;
using ManageProfile.DataAccess.Data;
using ManageProfile.Models;
using ManageProfile.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ManageProfileWeb.Controllers
{
    public class StateManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StateManagerController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            return View();

        }

        public JsonResult StateList()
        {
            var data = _unitOfWork.State.GetAllStates();
            return Json(data);
        }

        public JsonResult GetCountry()
        {
            var data = _unitOfWork.Country.GetAllCountry();
            return Json(data);
        }

        [HttpPost]
        public JsonResult AddState(State state)
        {
            if (ModelState.IsValid)
            {
                var existingState = _unitOfWork.State.Get(c => c.StateName == state.StateName);
                if (existingState != null)
                {
                    return new JsonResult(new { success = false, message = "State already exists" });
                }
                var obj = new State
                {
                    StateName = state.StateName,
                    CountryId = state.CountryId
                };
                _unitOfWork.State.AddState(state);
                _unitOfWork.Save();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Please enter required fields" });
            }
        }

        public JsonResult Edit(int stateId)
        {
            var data = _unitOfWork.State.Get(c => c.StateId == stateId);
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult EditState(State state)
        {
            if (ModelState.IsValid)
            {
                var existingState = _unitOfWork.State.Get(c => c.StateId != state.StateId && c.StateName == state.StateName);
                if (existingState != null)
                {
                    return new JsonResult(new { success = false, message = "State already exists" });
                }

                _unitOfWork.State.UpdateState(state);
                _unitOfWork.Save();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Please enter required fields" });
            }
        }

        public JsonResult Delete(int id)
        {
            var state = _unitOfWork.State.Get(c => c.StateId == id);

            var city = _unitOfWork.City.GetAll().Where(ci => ci.StateId == id).ToList();
            if (city.Count > 0)
            {
                return new JsonResult(new { success = false, message = "Cannot delete the state. Cities are available in the state." });
            }

            _unitOfWork.State.DeleteState(state);
            _unitOfWork.Save();
            return new JsonResult(new { success = true });
        }

        #region
        public IActionResult GetAll()
        {
            var objstate = _unitOfWork.State.GetAll(includeProperties: "Country").ToList();
            return Json(new { data = objstate });
        }
        #endregion
    }
}
