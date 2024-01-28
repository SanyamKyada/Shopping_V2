using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;
using System.Text.Json;

namespace Shopping.Controllers
{
    //[Authorize(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IProductService _productService;
        private readonly IVariantsService _variantService;
        private readonly IProductImageService _imageService;
        private readonly IAttributeService _attrService;
        private readonly ISKUAttributeService _skuAttrService;
        private readonly IWebHostEnvironment _enviorment;

        public SupplierController(IMenuService menuService, IAttributeService attrService, IProductService productService, ISKUAttributeService skuAttrService, IVariantsService variantsService, IProductImageService imageService, IWebHostEnvironment enviorment)
        {
            _menuService = menuService;
            _attrService = attrService;
            _skuAttrService = skuAttrService;
            _productService = productService;
            _variantService = variantsService;
            _imageService = imageService;
            _enviorment = enviorment;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (!User.IsInRole("Supplier"))
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/Supplier/Index" });
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(Vm_ProductWithVariants model)
        {
            string rootPath = $"/images/products/{model.ParentCategoryName}/{model.CategoryName}";
            for (int i = 0; i < model.Variants.Count; i++)
            {
                var dataList = JsonSerializer.Deserialize<List<Vm_Attribute>>(model.Variants[i].AttrsJSON);
            }

            var product = new ProductModel()
            {
                Name = model.ProductName,
                CategoryId = model.CategoryId,
                AspectRatioCssClass = "ratio3by2"
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
                    ThumbImage = $"{rootPath}/{Path.GetFileName(model.Images.FirstOrDefault(img => img.ColourId == model.Variants[i].ColorId).ThumbImage.FileName)}",
                    Colour = productColors.FirstOrDefault(c => c.Id == model.Variants[i].ColorId).Name,
                    Style = model.Variants[i].Style,
                    Description = model.Variants[i].Description,
                    IsMain = !encounteredColorIds.Contains(model.Variants[i].ColorId), // Logic not working. Need to test & debug
                    CreatedDate = DateTime.Now,
                    CommonSkuId = 0
                };
                if (!encounteredColorIds.Contains(model.Variants[i].ColorId))
                    encounteredColorIds.Add(model.Variants[i].ColorId);

                if (commonSkuIds.ContainsKey(model.Variants[i].ColorId))
                {
                    sku.CommonSkuId = commonSkuIds[model.Variants[i].ColorId];
                }
                else
                {
                    /// If not, assign a new CommonSkuId and store it in the dictionary
                    sku.CommonSkuId = currentMaxCommonSKUId + 1;
                    commonSkuIds[model.Variants[i].ColorId] = sku.CommonSkuId;
                    currentMaxCommonSKUId++;
                }

                //sku.ThumbImage = $"{rootPath}/{Path.GetFileName(model.Images.FirstOrDefault(img => img.ColourId == model.Variants[i].ColorId).ThumbImage.FileName)}";

                _variantService.Insert(sku);
                _variantService.Save();

                var attrList = JsonSerializer.Deserialize<List<Vm_Attribute>>(model.Variants[i].AttrsJSON);
                foreach (var attr in attrList)
                {
                    var skuAttr = new SKUAttributeModel()
                    {
                        SKUId = sku.Id,
                        AttributeMasterId = attr.idmaster,
                        Value = attr.attrval
                    };
                    _skuAttrService.Insert(skuAttr);
                }
            }
            _skuAttrService.Save();

            string wwwPath = _enviorment.WebRootPath;
            string path = string.Concat(_enviorment.WebRootPath, rootPath);
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
                    ImageUrl = $"{rootPath}/{fileName}",
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
                        ImageUrl = $"{rootPath}/{sideImageFileName}",
                        IsMain = false,
                        SKUId = 0,
                        CmnSkuId = commonSkuIds[variantImages.ColourId]
                    };
                    _imageService.Insert(image);
                }
            }
            _imageService.Save();

            return Ok(new {Message = "Product saved successfully." });
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

            _attrService.Insert(model);
            _attrService.Save();

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
        public IActionResult GetDetails()
        {
            var attrs = _attrService.GetAll();
            var colours = _productService.GetProductColours();
            return Json(new { attrsArray = attrs, productColoursArray = colours });
        }
    }
}
