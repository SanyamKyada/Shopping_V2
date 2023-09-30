using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult IndexCheckout()
        {
            return View();
        }
    }
}
