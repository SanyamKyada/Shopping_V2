using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
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

        public IActionResult ViewCart()
        {
            //var cartItems = _cartRepository.GetAllItems();
            return View(new Vm_CartItems());
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
