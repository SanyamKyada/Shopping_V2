namespace Shopping.Models.DTO
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? AspectRatioCssClass { get; set; }
		public int CategoryId { get; set; }
        // Collection of associated product images
        //public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<SKUModel> SKUs { get; set; }
    }
	public class ProductImage
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int SKUId { get; set; }
		public string ImageUrl { get; set; }
		public bool IsMain { get; set; }
		public int CmnSkuId { get; set; } // Cmn == Common

		// Navigation property for product
		//public ProductModel Product { get; set; }

	}
}
