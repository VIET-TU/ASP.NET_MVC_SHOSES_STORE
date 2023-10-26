using BTL_WEB_MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using ShopMobile.Data;
using ShopMobile.Models;

namespace ShopMobile.Controllers
{
	public class DashboardController : Controller
	{
        private ShopShoseDbContext db;

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
	}
}
