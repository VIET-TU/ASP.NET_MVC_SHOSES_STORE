using ShopMobile.Data;
using ShopMobile.Models;
using System.Security.Cryptography.X509Certificates;

namespace ShopMobile.Services
{
    public class UserService
    {
       

        public static User findOneUser(ShopShoseDbContext db, string email)
        {

            User user = db.Users.Where(u => u.email == email).FirstOrDefault();
            return user ;
        }

    
    }
}
