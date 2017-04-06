using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;

namespace Mahc_Final.Controllers
{
    public class UserController: Controller
    {
        private readonly mahcdbEntities _dbContext;

        public UserController()
        {
            _dbContext = new mahcdbEntities();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            UserCreateModel model = new UserCreateModel
            {
                UserRole = 0,
                UserRoles = _dbContext.Roles.OrderBy(r => r.Id)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(UserCreateModel userCreateModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    Username = userCreateModel.Username,
                    Password = userCreateModel.Password,
                    RoleId = userCreateModel.UserRole
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            // Failed.
            return RedirectToAction("Register");
        }

        [HttpGet]
        public ActionResult Users(string type)
        {
            if (type == null)
            {
                return View(_dbContext.Users);
            }
            else
            {
                return View(_dbContext.Users.Where(u => u.Role.Name == type));
            }
        }

        [HttpPost]
        public JsonResult UserExists(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            return Json(user == null);
        }
    }
}