using BTL_WEB_MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopMobile.Data;
using ShopMobile.Models;
using ShopMobile.Services;
using ShopMobile.utils;
using System.Collections.Generic;

namespace ShopMobile.Controllers
{
	public class DashboardController : Controller
	{
        private ShopShoseDbContext db;
        public static int limit = 5;

        public DashboardController(ShopShoseDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
		{
            User user = AuthController.CheckAuthentication(db, HttpContext);
            if(user != null)
            {
                if (user.RoleId == 1)
                {
                    return View();
                }
            }


            return RedirectToAction("Index", "Home");
        }

        public IActionResult UserManagement(int page = 1)
        {


            int offset = (page - 1) * limit;
            int count = db.Users.Count();
            int totalPage = (int)Math.Ceiling((decimal)count / limit);

            this.ViewBag.TotalPage = totalPage;
            this.ViewBag.CurrentPage = page;
            return View();
        }

        public IActionResult GetAllPanigationUser(int page = 1)
        {

            int offset = (page - 1) * limit;
            IEnumerable<User> users = db.Users.Skip(offset).Take(limit).Include(u => u.Role).ToList();
            return PartialView("TableUser", users);
        }

        public IActionResult CreateUser()
        {
            ViewBag.Role = new SelectList(db.Roles, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User newUser)
        {

            if (ModelState.IsValid)
            {

                User exitsUser = db.Users.Where(i => i.email.Equals(newUser.email)).FirstOrDefault();

                if (exitsUser != null)
                {
                    ModelState.AddModelError("email", "Email register is exits");
                    ViewBag.Role = new SelectList(db.Roles, "RoleId", "RoleName");
                    return View("CreateUser");
                }

                newUser.password = HandlePasswrod.hashPassword(newUser.password, null);

                newUser.avartar = "avartar_user.png";

                db.Users.Add(newUser);
                db.SaveChanges();
                return RedirectToAction("UserManagement");
            }
            ViewBag.Role = new SelectList(db.Roles, "RoleId", "RoleName");
            return View("CreateUser");
        }

        public IActionResult Update(int id)
        {
            User user = db.Users.Where(u => u.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Role = new SelectList(db.Roles, "RoleId", "RoleName");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User user)
        {
          
           if(ModelState.IsValid)
            {
                db.Users.Update(user);
                db.SaveChanges();
                return RedirectToAction("UserManagement");
            }
            ViewBag.Role = new SelectList(db.Roles, "RoleId", "RoleName");
            return View("Update");

        }

        public IActionResult Delete(int id)
        {

            User user = db.Users.Where(u => u.UserId == id).FirstOrDefault();

            // bổ sung xóa invoice của user dang muon xoa

            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("UserManagement");


        }
    }
}
