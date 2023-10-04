using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService<AttributeMasterModel> _attrMaster;
        private readonly DatabaseContext _context;
        public AdminController(IAdminService<AttributeMasterModel> attrMaster, DatabaseContext context)
        {
            _attrMaster = attrMaster;
            _context= context;
        }

        [HttpGet]
        public IActionResult AttributeMaster()
        {
            var categoris = new AttrMasterViewModel()
            {
                menuItems = _context.MenuTableNew.ToList()
            };
            return View(categoris);
        }
        [HttpGet]
        public IActionResult AddNewProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AttributeMaster(AttributeMasterModel model)
        {
            _attrMaster.Insert(model);

            return Ok();
        }
    }
}
