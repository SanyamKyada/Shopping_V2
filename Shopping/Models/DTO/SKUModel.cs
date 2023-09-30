namespace Shopping.Models.DTO
{
    public class SKUModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CommonSkuId { get; set; }
        public string SKUName { get; set; }
        public string Title { get; set; }
        public decimal OldPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public string ThumbImage { get; set; }
        public string Colour { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ICollection<SKUAttributeModel> SKUAttributeModels { get; set; }

        // Navigation property for product
        public ProductModel Product { get; set; }
    }
}
