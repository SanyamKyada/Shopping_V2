using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.DTO
{
    public class MenuItemsModelNew
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public string? ParentItemId { get; set; }
        public int ParentItemId { get; set; }

       

        //public List<MenuItemsModelNew> menuItems { get; set; }
        //public List<MenuItemsModelNew> menuItems = new List<MenuItemsModelNew>(); 
    }
}
