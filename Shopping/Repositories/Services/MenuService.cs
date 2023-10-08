using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using System.Linq.Expressions;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class MenuService : GenericRepository<MenuItemsModelNew>, IMenuService
    {
        private readonly DatabaseContext _context;
        public MenuService(DatabaseContext context) : base (context)
        {
            _context = context;
        }
        public List<MenuItemsModelNew> GetAllMenu()
        {
            return _context.MenuTableNew.ToList();
        }
    }
}
