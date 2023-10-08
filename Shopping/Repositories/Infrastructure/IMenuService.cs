using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IMenuService : IGenericRepository<MenuItemsModelNew>
    {
        public List<MenuItemsModelNew> GetAllMenu();
    }
}
