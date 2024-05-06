using Microsoft.AspNetCore.Mvc;
using ManageProfile.DataAccess.Data;
using ManageProfile.Models;
using ManageProfile.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManageProfileWeb.Controllers
{
    public class CityManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityManagerController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult CityList()
        {
            var data = _unitOfWork.City.GetAllCity().ToList();
            return Json(data);
        }
        public JsonResult GetCountry()
        {
            var data = _unitOfWork.Country.GetAllCountry();
            return Json(data);
        }

        public IActionResult GetState(int countryId)
        {
            var states = _unitOfWork.State.GetAll(s => s.CountryId == countryId)
                .Select(s => new { s.StateId, s.StateName })
                .ToList();

            return Json(states);
        }


        [HttpPost]
        public JsonResult AddCity(City city)
        {
            if (ModelState.IsValid)
            {
                var existingCity = _unitOfWork.City.Get(c => c.CityName == city.CityName);
                if (existingCity != null)
                {
                    return new JsonResult(new { success = false, message = "City already exists" });
                }
                var obj = new City
                {
                    CityName = city.CityName,
                    StateId = city.StateId,
                    CountryId = city.CountryId
                };
                _unitOfWork.City.AddCity(city);
                _unitOfWork.Save();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Please enter required fields" });
            }
        }

        public JsonResult Edit(int cityId)
        {
            var data = _unitOfWork.City.Get(c => c.CityId == cityId);
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult EditCity(City city)
        {
            if (ModelState.IsValid)
            {
                var existingCity = _unitOfWork.City.Get(c => c.CityId != city.CityId && c.CityName == city.CityName);
                if (existingCity != null)
                {
                    return new JsonResult(new { success = false, message = "City already exists" });
                }
                _unitOfWork.City.UpdateCity(city);
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
            var city = _unitOfWork.City.Get(c => c.CityId == id);

            var user = _unitOfWork.User.GetAll().Where(u => u.CityId == id).ToList();
            if (user.Count > 0)
            {
                return new JsonResult(new { success = false, message = "Cannot delete the city. Users are available in the city." });
            }

            _unitOfWork.City.DeleteCity(city);
            _unitOfWork.Save();
            return new JsonResult(new { success = true });
        }

        #region
        public IActionResult GetAll()
        {
            var objcity = _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList();
            return Json(new { data = objcity });
        }
        #endregion
    }
}
