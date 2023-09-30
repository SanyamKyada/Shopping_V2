using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.DTO
{
    public class ProductReviewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public int? Stars { get; set; }
        public int ParentReviewId { get; set; }
        public DateTime reviewDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string url { get; set; }
    }

    public class ReviewViewModel
    {
        public string reviewname { get; set; }
        public string reviewarea { get; set; }
        public string star1 { get; set; }
        public string productId { get; set; }
        public string parentId { get; set; }
        public string skuTitle { get; set; }
        public string skuName { get; set; }
        public string skuId { get; set; }
        public string selector { get; set; }
    }
}
