using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
	public class DetailsController : Controller
	{
		public IActionResult IndexDetails()
		{
			return View();
		}
	}
}
