using Shopping.Models.DTO;

namespace Shopping.Models.ViewModels
{
    public class Vm_SearchResults
    {
        public List<ProductModel> Products { get; set; }
        public List<SKUModel> SKU { get; set; }
        public string Query { get; set; }
    }
}
