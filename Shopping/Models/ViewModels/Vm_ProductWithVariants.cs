using Shopping.Controllers;

namespace Shopping.Models.ViewModels
{
    public class Vm_ProductWithVariants
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ImagesAspectRatio { get; set; }
        public List<Vm_VariantDetail> Variants { get; set; }
    }
}
