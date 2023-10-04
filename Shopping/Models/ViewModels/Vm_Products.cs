using Shopping.Models.DTO;

namespace Shopping.Models.ViewModels
{
    public class Vm_Products
    {
        public List<ProductModel> prodList { get; set; }
        public int pageIndex { get; set; }
        public int Count { get; set; }
    }
}
