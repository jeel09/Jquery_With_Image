using Microsoft.AspNetCore.Mvc;
using ManageProfile.DataAccess.Data;
using ManageProfile.Models;
using ManageProfile.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ManageProfileWeb.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserManagerController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult UserList()
        {
            var data = _unitOfWork.User.GetAllUser().ToList();
            return Json(data);
        }

        public JsonResult GetGender()
        {
            var data = _unitOfWork.Gender.GetAll();
            return Json(data);
        }

        public JsonResult GetHobby()
        {
            var data = _unitOfWork.Hobby.GetAll();
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

        public IActionResult GetCity(int stateId)
        {
            var cities = _unitOfWork.City.GetAll(c => c.StateId == stateId)
                .Select(c => new { c.CityId, c.CityName })
                .ToList();

            return Json(cities);
        }

        [HttpPost]
        public JsonResult AddUser(User user, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploads = Path.Combine(webRootPath, "images");
                    var filePath = Path.Combine(uploads, fileName);

                    if (!string.IsNullOrEmpty(user.ImageUrl))
                    {
                        var imagePath = Path.Combine(webRootPath, user.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    user.ImageUrl = @"\images\" + fileName;
                }

                var obj = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    GenderId = user.GenderId,
                    HobbyId = string.Join(", ", user.HobbyId), // Separate hobby ids with comma
                    CountryId = user.CountryId,
                    StateId = user.StateId,
                    CityId = user.CityId,
                    ImageUrl = user.ImageUrl
                };
                _unitOfWork.User.AddUser(user);
                _unitOfWork.Save();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Please enter required fields" });
            }
        }

        public JsonResult Edit(int userId)
        {
            var data = _unitOfWork.User.Get(c => c.UserId == userId);
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult EditUser(User user, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploads = Path.Combine(webRootPath, "images");
                    var filePath = Path.Combine(uploads, fileName);

                    if(!string.IsNullOrEmpty(user.ImageUrl))
                    {
                        var imagePath = Path.Combine(webRootPath,user.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    user.ImageUrl = @"\images\" + fileName;
                }

                _unitOfWork.User.UpdateUser(user);
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
            var user = _unitOfWork.User.Get(c => c.UserId == id);
            _unitOfWork.User.DeleteUser(user);
            _unitOfWork.Save();
            return new JsonResult(new { success = true });
        }

        #region
        public IActionResult GetAll()
        {
            var objuser = _unitOfWork.User.GetAll(includeProperties: "Gender,Country,State,City").ToList();
            return Json(new { data = objuser });
        }
        #endregion
    }
}
