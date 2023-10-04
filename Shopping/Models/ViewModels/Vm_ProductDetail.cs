using Shopping.Models.DTO;

namespace Shopping.Models.ViewModels
{
    public class Vm_ProductDetail
    {
        public SKUModel _SKU { get; set; }
        public List<SKUModel> _SKUList { get; set; }
        public List<SKUModel> relatedSKUList { get; set; }
        public ProductModel _Product { get; set; }
        public List<ProductImage> SKUImages = new List<ProductImage>();
        public List<SKUAttributeModel> skuAttrs { get; set; }
        public List<AttributeMasterModel> attrMstrs = new List<AttributeMasterModel>();
        public List<ProductReviewModel> ReviewModels = new List<ProductReviewModel>();
        public string? currentUserName { get; set; }
    }
}
