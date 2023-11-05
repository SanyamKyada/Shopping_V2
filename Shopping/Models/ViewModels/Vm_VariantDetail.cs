namespace Shopping.Models.ViewModels
{
    public class Vm_VariantDetail
    {
        public string VariantName { get; set; }
        public string SKU { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumInventoryAlert { get; set; }
        public IFormFile ThumbImage { get; set; }
        public int ColorId { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
    }
}
