using BTL_WEB_MVC.Controllers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopMobile.Data;
using ShopMobile.Models;
using ShopMobile.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShopMobile.Controllers
{
    public class HomeController : Controller
    {
        public static int limit = 6;
        public static int lst_product_limit = 6;

        private ShopShoseDbContext db;

        public HomeController(ShopShoseDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = db.Products.OrderByDescending(p => p.ProductId).Take(lst_product_limit).ToList();
            ViewBag.TotalProduct = db.Products.Count();
            return View(products);
        }

        public IActionResult GetLatestProductWithPanigation(int cursorId)
        {
            IEnumerable<Product> products = db.Products
                                         .Where(p => p.ProductId <= cursorId)
                                         .OrderByDescending(p => p.ProductId)
                                         .Take(lst_product_limit)
                                         .ToList();
            return PartialView("ListLatestProduc", products);
        }

        public IActionResult Shop(int page = 1)
        {
            IEnumerable<Product> products = db.Products.ToList();
            IEnumerable<Category> categories = db.Categories.ToList();
            List<CategoryData> categoryDataList = new List<CategoryData>();

            foreach (Category category in categories)
            {
                int cuont_ = category.Products.Count();
                CategoryData categoryData = new CategoryData
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    ProductCount = cuont_
                };
                categoryDataList.Add(categoryData);
            }

            int offset = (page - 1) * limit;
            int count = products.Count();
            int totalPage = (int)Math.Ceiling((decimal)count / limit);
            products = products.OrderBy(p => p.ProductId).Skip(offset).Take(limit).ToList();

            ViewBag.CategoryDataList = categoryDataList;
            ViewBag.TotalPage = totalPage;
            ViewBag.TotoalCountProduct = count;
            ViewBag.Page_active = page;
            return View(products);
        }

        public IActionResult GetAllProductByCategory(int? categoryId = null, int page = 1)
        {
            IEnumerable<Product> products;
            if (categoryId == null)
            {
                products = db.Products.ToList();
            } else
            {

            products = db.Products.Where(p => p.CategoryId == categoryId).ToList();
            }

    
            int offset = (page - 1) * limit;
            int count = products.Count();
            int totalPage = (int)Math.Ceiling((decimal)count / limit);
            products = products.OrderBy(p => p.ProductId).Skip(offset).Take(limit).ToList();
            ViewBag.TotalPage = totalPage;
            ViewBag.Page_active = page;
            return PartialView("ListProduct", products);
        }



   


        public IActionResult ShopSingle(int id)
        {
            Product product = db.Products.Where(p => p.ProductId == id).Include(p => p.Category).FirstOrDefault();   
            return View(product);
        }

        public IActionResult ListSimilarProduct(int productId,string category)
        {
            IEnumerable<Product> products = db.Products.Where(p =>  p.ProductId != productId ).OrderBy(r => Guid.NewGuid()).Take(6).ToList();

            return PartialView("ListSimilarProducts", products);
        }




    }

    public class CategoryData
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }
}


