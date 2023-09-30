﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Models;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using System.Diagnostics;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Controllers
{

    public class HomeController : Controller
    {
        // private readonly IMenuRepo _menuRepo;
        private readonly DatabaseContext _context;
        private readonly IServiceProvider _serviceProvider;
        public HomeController(DatabaseContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {

            var productIdAspectRatioMap = _context.Products
                .Select(p => new { p.Id, p.AspectRatioCssClass })
                .ToList();

            var skus = _context.SKUs.OrderByDescending(x => x.CreatedDate).ToList();
            var model = new HomePageViewModel()
            {
                SKUModels = _context.SKUs.ToList(),
                FeaturedProductsModels = _context.FeaturedProducts.ToList(),
                BannerModels = _context.BannerProducts.Where(bm => bm.IsActive == true).OrderByDescending(bm => bm.Id).ToList(),
                LatestSKUs = skus.DistinctBy(x => x.ProductId).ToList(),
                ProductIdAspectRatioMap = _context.Products
                    .Select(p => new { p.Id, p.AspectRatioCssClass })
                    .ToDictionary(p => p.Id, p => p.AspectRatioCssClass)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Menu()
        {
            /*var _menuModel = _context.MenuTableNew.ToList();

            var model = new MenuItemsModelNew
            {
                menuItems = _menuModel
            };*/

            var model = _context.MenuTableNew.ToList();
            return PartialView("_MenuPartial", model);
        }

    }

    public class HomePageViewModel
    {
        public List<BannerModel> BannerModels { get; set; }
        public List<FeaturedProductsModel> FeaturedProductsModels { get; set; }
        public List<SKUModel> SKUModels = new List<SKUModel>();
        public List<SKUModel> LatestSKUs = new List<SKUModel>();
        public Dictionary<int, string> ProductIdAspectRatioMap { get; set; }
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string AspectRatioCssClass { get; set; }
    }
}