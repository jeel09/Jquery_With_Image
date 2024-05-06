using Microsoft.AspNetCore.Mvc;
using ManageProfile.DataAccess.Data;
using ManageProfile.Models;
using ManageProfile.DataAccess.Repository.IRepository;

namespace ManageProfileWeb.Controllers
{
    public class CountryManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CountryManagerController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult CountryList()
        {
            var data = _unitOfWork.Country.GetAllCountry().ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult AddCountry(Country country)
        {
            if (ModelState.IsValid)
            {
                var existingCountry = _unitOfWork.Country.Get(c => c.CountryName == country.CountryName);
                if (existingCountry != null)
                {
                    return new JsonResult(new { success = false, message = "Country already exists" });
                }

                var obj = new Country
                {
                    CountryName = country.CountryName
                };
                _unitOfWork.Country.AddCountry(obj);
                _unitOfWork.Save();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Please enter required fields" });
            }
        }

        public JsonResult Edit(int id)
        {
            var data = _unitOfWork.Country.Get(c => c.CountryId == id);
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult EditCountry(Country country)
        {
            if (ModelState.IsValid)
            {
                var existingCountry = _unitOfWork.Country.Get(c => c.CountryId != country.CountryId && c.CountryName == country.CountryName);
                if (existingCountry != null)
                {
                    return new JsonResult(new { success = false, message = "Country already exists" });
                }
                _unitOfWork.Country.UpdateCountry(country);
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
            var country = _unitOfWork.Country.Get(c => c.CountryId == id);

            var states = _unitOfWork.State.GetAll().Where(s => s.CountryId == id).ToList();
            if (states.Count > 0)
            {
                return new JsonResult(new { success = false, message = "Cannot delete the country. States are available in the country." });
            }

            _unitOfWork.Country.DeleteCountry(country);
            _unitOfWork.Save();
            return new JsonResult(new { success = true });
        }
    }
}
