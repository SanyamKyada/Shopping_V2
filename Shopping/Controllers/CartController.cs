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
        public IActionResult getCartPopup(Guid UserId, string sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                var model = _cartRepository.GetAllByUserId(UserId);
                return PartialView("_addToCartPartial", model);
            }
            else
            {
                var cartItems = _cartRepository.GetAllByUserId(UserId);

                //switch (sortBy)
                //{
                //    case "price":
                //        cartItems = cartItems.OrderBy(p => p.Price).ToList();
                //        break;
                //    case "name":
                //        cartItems = cartItems.OrderBy(p => p.Name).ToList();
                //        break;
                //    default:
                //        break;
                //}
                return PartialView("_addToCartPartial", cartItems);
            }
        }
        [HttpPost]
        public IActionResult AddToCart(CartModel model)
        {
            _cartRepository.AddToCart(model.SKUId, model.Quantity, model.UserId);
            return Json(new {UserId = model.UserId });
            //return RedirectToAction("getCartPopup");     
        }
        public IActionResult RemoveCartItem(int id)
        {
            _cartRepository.Delete(id);
            _cartRepository.Save();
            return RedirectToAction("ViewCart", "Cart");

        }
        //[HttpGet]
        //public IActionResult getSortedCartPopup(List<CartModel> cartItems)
        //{
        //    return PartialView("_addToCartPartial", cartItems);
        //}
        //[HttpPost]
        //public IActionResult SortCart(string sortBy)
        //{
        //    var cartItems = _cartRepository.GetAllItems();

        //    switch (sortBy)
        //    {
        //        case "price":
        //            cartItems = cartItems.OrderBy(p => p.Price).ToList();
        //            break;
        //        case "name":
        //            cartItems = cartItems.OrderBy(p => p.Name).ToList();
        //            break;
        //        default:
        //            break;
        //    }

        //    //return PartialView("_addToCartPartial", cartItems);
        //    return Json(new { });
        //}
    }
}
