using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IMenuService
    {
        public List<MenuItemsModelNew> GetAllMenu();
    }
}
