using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;
using Shopping.Repositories.Services;
using System.Text.Json.Serialization;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Controllers
{
    public class AdminController : Controller
    {
        //private readonly IAdminService<AttributeMasterModel> _attrMaster;
        private readonly IMenuService _menuService;
        private readonly IProductService _productService;
        private readonly IVariantsService _variantService;
        private readonly IProductImageService _imageService;
        private readonly IAttributeService _attrService;
        private readonly IWebHostEnvironment _enviorment;

        public AdminController(IMenuService menuService, IAttributeService attrService,IProductService productService, IVariantsService variantsService, IProductImageService imageService, IWebHostEnvironment enviorment)
        {
            _menuService = menuService;
            _attrService = attrService;
            _productService = productService;
            _variantService = variantsService;
            _imageService = imageService;
            _enviorment = enviorment;
        }

        [HttpGet]
        public IActionResult AttributeMaster()
        {
            var categoris = new AttrMasterViewModel()
            {
                menuItems = _menuService.Get(filter: category => category.ParentItemId > 1, orderBy: category => category.OrderBy(x => x.Name)).ToList()
            };
            return View(categoris);
        }

        [HttpPost]
        public IActionResult AttributeMaster(AttributeMasterModel formData)
        {
            var model = new AttributeMasterModel()
            {
                CategoryId = formData.CategoryId,
                ComponentType = "Dropdown",
                IsSpecificationAttribute = false,
                Lable = formData.Lable,
                Values = formData.Values
            };

            //_attrService.Insert(model);
            //_attrService.Save();

            return Ok();
        }
        [HttpGet]
        public IActionResult SpecificationAttribute()
        {
            var categoris = new AttrMasterViewModel()
            {
                menuItems = _menuService.Get(filter: category => category.ParentItemId > 1, orderBy: category => category.OrderBy(x => x.Name)).ToList()
            };
            return View(categoris);
        }

        [HttpPost]
        public IActionResult SpecificationAttribute(Dto_SpecificationAttributes formData)
        {
            var model = new AttributeMasterModel()
            {
                CategoryId = formData.CategoryId,
                ComponentType = formData.ComponentType,
                IsSpecificationAttribute = true,
                Lable = formData.Lable,
                Values = formData.Values
            };

            _attrService.Insert(model);
            _attrService.Save();

            return Ok();
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(Vm_ProductWithVariants model)
        {
            var product = new ProductModel()
            {
                Name = model.ProductName,
                CategoryId = model.CategoryId,
                AspectRatioCssClass = ""
            };
            _productService.Insert(product);
            _productService.Save();

            var productColors = _productService.GetProductColours();

            int currentMaxCommonSKUId = _variantService.GetMaxCommonSKUId();
            var commonSkuIds = new Dictionary<int, int>();
            var encounteredColorIds = new List<int>();

            for (int i = 0; i < model.Variants.Count; i++)
            {
                var sku = new SKUModel()
                {
                    ProductId = product.Id,
                    SKUName = model.Variants[i].SKU,
                    Title = model.Variants[i].VariantName,
                    OldPrice = model.Variants[i].OriginalPrice,
                    SellingPrice = model.Variants[i].SalePrice,
                    Quantity = model.Variants[i].QuantityInStock,
                    MinQuantity = model.Variants[i].MinimumInventoryAlert,
                    ThumbImage = "",
                    Colour = productColors.FirstOrDefault(c => c.Id == model.Variants[i].ColorId).Name,
                    Style = model.Variants[i].Style,
                    Description = model.Variants[i].Description,
                    IsMain = encounteredColorIds.Contains(model.Variants[i].ColorId),
                    CreatedDate = DateTime.Now,
                    CommonSkuId = 0
                };
                sku.IsMain = encounteredColorIds.Contains(model.Variants[i].ColorId);

                if (commonSkuIds.ContainsKey(model.Variants[i].ColorId))
                {
                    sku.CommonSkuId = commonSkuIds[model.Variants[i].ColorId];
                }
                else
                {
                    // If not, assign a new CommonSkuId and store it in the dictionary
                    sku.CommonSkuId = currentMaxCommonSKUId + 1;
                    commonSkuIds[model.Variants[i].ColorId] = sku.CommonSkuId;
                    currentMaxCommonSKUId++;
                }

                sku.ThumbImage = $"/images/products/{model.ParentCategoryName}/{model.CategoryName}/{Path.GetFileName(model.Images.FirstOrDefault(img => img.ColourId == model.Variants[i].ColorId).ThumbImage.FileName)}";

                _variantService.Insert(sku);
            }
            _variantService.Save();

            string wwwPath = _enviorment.WebRootPath; //"D:\\Sanyam\\Playground\\Shopping_V2\\Shopping\\wwwroot"
            string path = Path.Combine(_enviorment.WebRootPath, $"images/products/{model.ParentCategoryName}/{model.CategoryName}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            List<string> uploadedFiles = new List<string>();
            foreach (var variantImages in model.Images)
            {
                /// Thumb Image
                string fileName = Path.GetFileName(variantImages.ThumbImage.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    variantImages.ThumbImage.CopyTo(stream);
                }
                ProductImage thumbImage = new()
                {
                    ProductId = product.Id,
                    ImageUrl = $"/images/products/{model.ParentCategoryName}/{model.CategoryName}/{fileName}",
                    IsMain = true,
                    SKUId = 0,
                    CmnSkuId = commonSkuIds[variantImages.ColourId]
                };
                _imageService.Insert(thumbImage);

                /// Side Images
                foreach (IFormFile sideImage in variantImages.SideImages)
                {

                    string sideImageFileName = Path.GetFileName(sideImage.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, sideImageFileName), FileMode.Create))
                    {
                        sideImage.CopyTo(stream);
                    }
                    ProductImage image = new()
                    {
                        ProductId = product.Id,
                        ImageUrl = $"/images/products/{model.ParentCategoryName}/{model.CategoryName}/{sideImageFileName}",
                        IsMain = false,
                        SKUId = 0,
                        CmnSkuId = commonSkuIds[variantImages.ColourId]
                    };
                    _imageService.Insert(image);
                }
            }
            _imageService.Save();

            return Ok();
        }

        [HttpGet]
        public IActionResult GetDetails()
        {
            var attrs = _attrService.GetAll();
            var colours = _productService.GetProductColours();
            return Json(new { attrsArray = attrs, productColoursArray = colours});
        }

        [HttpGet]
        public IActionResult TestAddNewProduct()
        {
            return View();
        }
    }
}
