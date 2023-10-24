using BTL_WEB_MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopMobile.Data;
using ShopMobile.Models;
using ShopMobile.Services;
using System.Collections.Generic;

namespace ShopMobile.Controllers
{
    public class ProductController : Controller
    {
        private ShopShoseDbContext db;

        public ProductController(ShopShoseDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1)
        {
            int limit = 5;
            int count = db.Products.Count();
            int totalPage = (int)Math.Ceiling((decimal)count / limit);
            
            Pager pager = new Pager(page, totalPage, limit);

            this.ViewBag.Pager = pager;
            return View();
        }

        public IActionResult GetAllPanigationProduct(int page = 1)
        {
            int limit = 5;
            int offset = (page - 1) * limit;
            IEnumerable<Product> products = db.Products.OrderBy(p => p.ProductId).Skip(offset).Take(limit).ToList();
            return PartialView("ListProductTable", products);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            User user = UserService.findOneUser(db, AuthController.CheckAuthentication(HttpContext));
            if (user == null)
            {
                TempData["error"] = "Create precate deny permission";
                return View("Create");

            }

            product.UserId = user.UserId;

            if (ModelState.IsValid)
            {
                

                db.Products.Add(product);
                db.SaveChanges();
                TempData["success"] = "Create a new product successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Create a new product false";

            return View("Create");
        }


        public IActionResult Edit(int id) 
        {
            if(id == null || db.Products == null)
            {
                return NotFound();
            } 
            
             Product product = ProductService.getOneById(db, id);
            if (product == null)
            {
                return NotFound();
            }
              return View(product);
                      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            User user = UserService.findOneUser(db, AuthController.CheckAuthentication(HttpContext));
            if (user == null)
            {
                TempData["error"] = "You need to login";
                return View("Edit");
            }

            if (product.ProductId == null) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    product.UserId = user.UserId;
                    db.Update(product);
                    db.SaveChanges();
                } catch (DbUpdateConcurrencyException)
                {
                    return  NotFound(); 
                }
            return RedirectToAction("Index");
            }

            return View(product);

        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            
            Product product = ProductService.getOneById(db, id);
            if (product == null)
            {
                return Json ( new { success = false} );
            }
            db.Products.Remove(product);
            db.SaveChanges();

            return Json(new { success = true });


        }

    }
}
