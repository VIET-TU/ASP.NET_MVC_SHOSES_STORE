using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShopMobile.Data;
using ShopMobile.Models;
using System.Collections.Generic;

namespace ShopMobile.Services
{
    public class ProductService
    {
        public static IEnumerable<Product> getAllProduct(ShopShoseDbContext db)
        {
            IEnumerable<Product> products = db.Products.ToList();
            return products;
        }

        public static Product getOneById(ShopShoseDbContext db, int id) { 
            return db.Products.Where(i => i.ProductId == id).FirstOrDefault();
        }

        public static IEnumerable<Product> getProductWithPanigation(ShopShoseDbContext db, int page, int limit = 5) {
          
            return null;
        }

        public static int getTotalPageProduct(ShopShoseDbContext db,int limit)
        {
            int realLimit = Math.Min(5, limit);
            int count = db.Products.Count();
            int totalPage = (int) Math.Ceiling((decimal)count / realLimit);
            
            return totalPage;
        }

    }
}
