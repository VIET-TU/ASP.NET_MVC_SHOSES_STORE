using BTL_WEB_MVC.Controllers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using ShopMobile.Data;
using ShopMobile.Models;
using ShopMobile.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShopMobile.Controllers
{
    public class HomeController : Controller
    {
        private ShopShoseDbContext db;

        public HomeController(ShopShoseDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {

            string email = AuthController.CheckAuthentication(HttpContext) as string;
            User user = UserService.findOneUser(db, email);
            ViewBag.user = user;
            return View();


        }

        public IActionResult Shop()
        {
            IEnumerable<Product> products = db.Products.ToList();
            
            return View(products);
        }

        public IActionResult ShopSingle(int id)
        {
            Product product = db.Products.Where(p => p.ProductId == id).FirstOrDefault();   
            return View(product);
        }

        public IActionResult ListSimilarProduct(int productId,string category)
        {
            IEnumerable<Product> products = db.Products.Where(p => p.Category == category && p.ProductId != productId).ToList();

            return PartialView("ListSimilarProducts", products);
        }


    }
}