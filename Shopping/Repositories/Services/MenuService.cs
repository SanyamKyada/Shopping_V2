using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class MenuService : IMenuService
    {
        private readonly DatabaseContext _context;
        public MenuService(DatabaseContext context)
        {
            _context = context;
        }
        public List<MenuItemsModelNew> GetAllMenu()
        {
            return _context.MenuTableNew.ToList();
        }
    }
}
