using BTL_WEB_MVC.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ShopMobile.Data;
using ShopMobile.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ShopMobile.Controllers
{
    public class InvoiceController : Controller
    {
        private ShopShoseDbContext db;

        public InvoiceController(ShopShoseDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {

            User user = AuthController.CheckAuthentication(db, HttpContext);

            Invoice latestInvoice = db.Invoices
                               .Where(c => c.UserId == user.UserId && !c.Status)
                               .FirstOrDefault();
            if (latestInvoice == null)
            {
                latestInvoice = this.CreateNewInvoice(user.UserId);
            }

            IEnumerable<Invoice_Products> list_product_invoice = db.Invoice_Products.Where(i => i.InvoiceId == latestInvoice.InvoiceId).Include(i => i.Product).ToList();
            int totalPrice = 0;
            foreach (var ip in list_product_invoice)
            {
                totalPrice += int.Parse(ip.totalPrice);
            }
            ViewBag.TotalPrice = totalPrice;
            return View(list_product_invoice);
        }

        private Invoice CreateNewInvoice(int userId)
        {
            Invoice newInvoice = new Invoice()
            {
                UserId = userId,
                Status = false,
            };
            db.Invoices.Add(newInvoice);
            db.SaveChanges();

            return newInvoice;
        }

        public IActionResult addToInvoice(int ProductId, int quantity, string size)
        {
            if (ModelState.IsValid)
            {
                User user = AuthController.CheckAuthentication(db, HttpContext);
                if (user == null)
                {
                    return RedirectToAction("Auth", "Login");
                }

                Product info_product = db.Products.Where(p => p.ProductId == ProductId).FirstOrDefault();
                if (info_product == null)
                {
                    return NotFound();
                }

                if (info_product.quantity < quantity)
                {
                    return Json(new { success = false, message = "Products sold through warehouses" });
                }

                Invoice latestInvoice = db.Invoices
                               .Where(c => c.UserId == user.UserId && !c.Status)
                               .FirstOrDefault();
                if (latestInvoice == null)
                {
                    latestInvoice = this.CreateNewInvoice(user.UserId);
                }


                var invoice_detail_exit = db.Invoice_Products
                             .Where(i => i.ProductId == ProductId && i.InvoiceId == latestInvoice.InvoiceId && i.size == size)
                             .FirstOrDefault();


                if (invoice_detail_exit != null)
                {
                    invoice_detail_exit.quantity += quantity;
                    invoice_detail_exit.totalPrice = (int.Parse(info_product.price) * invoice_detail_exit.quantity).ToString();
                }
                else
                {
                    db.Invoice_Products.Add(new Invoice_Products()
                    {
                        InvoiceId = latestInvoice.InvoiceId,
                        ProductId = ProductId,
                        quantity = quantity,
                        size = size,
                        totalPrice = (int.Parse(info_product.price) * quantity).ToString(),
                    });
                }


                int totalQuantity = db.Invoice_Products
                                    .Where(i => i.InvoiceId == latestInvoice.InvoiceId)
                                    .Sum(q => q.quantity);




                db.SaveChanges();

                return Json(new { success = true, message = "Add to invoice success", cart_quantity = totalQuantity });
            }

            return Json(new { success = false, message = "Add to invoice false" });

        }

        public static int CardTotalQuantity(ShopShoseDbContext db, int userId)
        {
            Invoice latestInvoice = db.Invoices
                               .Where(c => c.UserId == userId && !c.Status)
                               .FirstOrDefault();
            int totalQuantity = 0;

            if (latestInvoice != null)
            {
                totalQuantity = db.Invoice_Products
                                  .Where(i => i.InvoiceId == latestInvoice.InvoiceId)
                                  .Sum(q => q.quantity);
            }



            return totalQuantity;
        }

        [HttpPost]
        public IActionResult DeleteItemInvoiceProduct(int id)
        {
            Invoice_Products ip = db.Invoice_Products.Where(ip => ip.Id == id).FirstOrDefault();
            if (ip != null)
            {
                db.Invoice_Products.Remove(ip);
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        public IActionResult Checkout()
        {
            User user = AuthController.CheckAuthentication(db, HttpContext);
            Invoice latestInvoice = db.Invoices
                              .Where(c => c.UserId == user.UserId && !c.Status)
                              .FirstOrDefault();
            ViewBag.User = user;
            ViewBag.Invoice = latestInvoice;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(int invoiceID)
        {
            if(invoiceID != null) { 
                Invoice invoice = db.Invoices.Where(i => i.InvoiceId == invoiceID).FirstOrDefault();
                if (invoice != null)
                {
                    invoice.Status = true;
                    db.SaveChanges();
                    return RedirectToAction("ThankYou");
                }
            }
            return View("Checkout");

        }

        public IActionResult ThankYou()
        {
            
            return View();
        }



    }

   
}
