using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopping.Models;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;
using Shopping.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Route("~/Product/{categoryId}/{currentPage}")]
        [HttpGet]
        public async Task<IActionResult> Index(int categoryId, int currentPage)
        {
            try
            {
                int pageSize = 8;

                var products = await _productService.GetPaginatedProductsAsync(categoryId, currentPage, pageSize);

                var model = new Vm_Products
                {
                    prodList = products,
                    Count = await _productService.GetTotalPageCountAsync(categoryId, pageSize),
                    pageIndex = currentPage
                };

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching products.");
                throw;
            }
        }

        [Route("~/ProductDetails/{title}/{skuName}/{id}/{selector}")]
        [HttpGet]
        public async Task<IActionResult> ProductDetails(string title, string skuName, int id, int selector)
        {
            if (id != 0)
            {
                var userN = User.Identity.Name;
                var userName = await _productService.GetUserNameAsync(userN);

                var model = await _productService.GetProductDetailsAsync(id);
                model.currentUserName = userName;

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReviewSubmit(ReviewViewModel model)
        {
            if (model.star1 == null || model.star1 == string.Empty)
                model.star1 = "0";

            int rating = int.Parse(model.star1);
            int prodId = int.Parse(model.productId);
            int parentId = int.Parse(model.parentId);
            int skuId = int.Parse(model.skuId);

            try
            {
                await _productService.AddProductReviewAsync(prodId, model.reviewname, model.reviewarea, rating, parentId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while submitting the review.");
                throw;
            }

            return RedirectToAction("ProductDetails", new { title = model.skuTitle, skuName = model.skuName, Id = skuId, selector = model.selector });
        }
    }
}
