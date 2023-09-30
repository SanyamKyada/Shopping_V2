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

	public class ProductIndexViewModel
	{
		public List<ProductModel> prodList{get; set;}
        public int pageIndex { get; set; }
        public int Count { get; set; }
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

	public class SKUViewModel
	{
		public SKUModel _SKU { get; set; }
		public List<SKUModel> _SKUList { get; set; }
		public List<SKUModel> relatedSKUList { get; set; }
		public ProductModel _Product { get; set; }
        public List<ProductImage> SKUImages = new List<ProductImage>();
		public List<SKUAttributeModel> skuAttrs { get; set; }
		public List<AttributeMasterModel> attrMstrs = new List<AttributeMasterModel>();
        public List<ProductReviewModel> ReviewModels = new List<ProductReviewModel>();
		public string ?currentUserName { get; set; }
    }

	public class RelatedProductsModel
	{
		public ProductModel Product { get; set; }

		public List<ProductModel> RelatedProducts = new List<ProductModel>();
		//public List<SKUModel> SKU = new List<SKUModel>();
	}

	public class ProductViewModel
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal SellingPrice { get; set; }
		public string ImageUrl { get; set; }
	}
}
