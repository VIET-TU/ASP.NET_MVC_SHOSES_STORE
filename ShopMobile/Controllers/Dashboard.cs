using Microsoft.AspNetCore.Mvc;

namespace ShopMobile.Controllers
{
	public class Dashboard : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
