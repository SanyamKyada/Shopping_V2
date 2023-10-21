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
        private readonly IAttributeService _attrService;
        public AdminController(IMenuService menuService, IAttributeService attrService)
        {
            _menuService = menuService;
            _attrService = attrService;
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
        public IActionResult AddNewProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(Vm_ProductWithVariants model)
        {
            return Ok();
        }


    }
}
