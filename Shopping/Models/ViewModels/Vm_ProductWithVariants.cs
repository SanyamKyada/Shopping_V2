using Shopping.Controllers;

namespace Shopping.Models.ViewModels
{
    public class Vm_ProductWithVariants
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string ImagesAspectRatio { get; set; }
        public List<Vm_VariantDetail> Variants { get; set; }
        public List<Vm_VariantImages> Images { get; set; }
    }

    public class Vm_VariantImages
    {
        public int ColourId { get; set; }
        public IFormFile ThumbImage { get; set; }
        public List<IFormFile> SideImages { get; set; }
    }
}
