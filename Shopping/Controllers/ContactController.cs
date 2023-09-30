using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult IndexContact()
        {
            return View();
        }
    }
}
