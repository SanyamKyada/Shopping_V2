using Shopping.Models.DTO;

namespace Shopping.Models.ViewModels
{
    public class Vm_HomePage
    {
        public List<BannerModel> BannerModels { get; set; }
        public List<FeaturedProductsModel> FeaturedProductsModels { get; set; }
        public List<SKUModel> SKUModels = new List<SKUModel>();
        public List<SKUModel> LatestSKUs = new List<SKUModel>();
        public Dictionary<int, string> ProductIdAspectRatioMap { get; set; }
    }
}
