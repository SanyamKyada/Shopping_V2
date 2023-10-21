namespace Shopping.Models.DTO
{
    public class AttributeMasterModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Lable { get; set; }
        public string ComponentType { get; set; }
        public bool IsSpecificationAttribute { get; set; }
        public string ?Values { get; set; }
        // Navigation property
        //public SKUModel SKUModelNav { get; set; }
    }

    public class AttrMasterViewModel
    {
        public AttributeMasterModel attrModel { get; set; }
        public List<MenuItemsModelNew> menuItems { get; set; }
    }
}
