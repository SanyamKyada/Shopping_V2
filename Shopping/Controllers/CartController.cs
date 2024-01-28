using Microsoft.AspNetCore.Mvc;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartRepository;

        public CartController(ICartService cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IActionResult ViewCart(Guid UserId)
        {
            var cartItems = _cartRepository.GetAllByUserId(UserId);
            return View(cartItems);
        }
        [HttpGet]
        public IActionResult getCartPopup(Guid UserId)
        {
            var model = _cartRepository.GetAllByUserId(UserId);
            return PartialView("_addToCartPartial", model);
        }
        [HttpPost]
        public IActionResult AddToCart(CartModel model)
        {
            _cartRepository.AddToCart(model.SKUId, model.Quantity, model.UserId);
            return Json(new { UserId = model.UserId });
            //return RedirectToAction("getCartPopup");     
        }
        public IActionResult RemoveCartItem(int id)
        {
            _cartRepository.Delete(id);
            _cartRepository.Save();
            return RedirectToAction("ViewCart", "Cart");
        }
    }
}
