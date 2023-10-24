using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using ShopMobile.Data;
using ShopMobile.Models;
using ShopMobile.Services;
using ShopMobile.utils;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BTL_WEB_MVC.Controllers
{
    public class AuthController : Controller
    {
        private ShopShoseDbContext db;

        public AuthController(ShopShoseDbContext db)
        {
            this.db = db;
        }

        public IActionResult Login()
        {

            return View();
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(Login login)
		{
            if (ModelState.IsValid)
            {
				User exitsUser = db.Users.Where(i => i.email.Equals(login.email.Trim())).FirstOrDefault();

				if (User != null)
				{
					if(HandlePasswrod.verifyPassword(exitsUser.password, login.password))
					{
						HttpContext.Session.SetString("email", login.email);

                        TempData["success"] = "Login successfully";
                        return RedirectToAction("Index", "Home");
					}
				}

				TempData["false"] = "Login false";
				ModelState.AddModelError("email", "Email hoặc password không hợp lệ");


                    return View("Login");
				
			}
                return View("Login");
		}

		public IActionResult Register()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(Register register)
		{

			if (ModelState.IsValid) { 

                User exitsUser = db.Users.Where(i => i.email.Equals(register.email.Trim())).FirstOrDefault();

                if(exitsUser != null)
                {
					ModelState.AddModelError("email", "Email đăng ký đã tồn tại");
					TempData["success"] = "Register successfully";
					return View("Register");
				}

                if(register.password.Equals(register.confirmPassword))
                {
                    register.password = HandlePasswrod.hashPassword(register.password, null);
                    register.address = "";
					register.avartar = "";

					db.Users.Add(register);
                    db.SaveChanges();
					TempData["false"] = "Register successfully";
					return RedirectToAction("Login");   
				}
            }
			TempData["false"] = "Register false";
			return View("Register");



		}

        public IActionResult CheckAuthentication()
        {
            string email = HttpContext.Session.GetString("email");

            User user = UserService.findOneUser(db, email);
            return new JsonResult(user);
        }


        public static string CheckAuthentication(HttpContext httpContext)
        {
            string email = httpContext.Session.GetString("email");

            return email;
        }



    }
}
