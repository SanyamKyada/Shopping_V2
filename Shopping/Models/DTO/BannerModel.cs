namespace Shopping.Models.DTO
{
    public class BannerModel
    {
        public int Id { get; set; }
        public int SKUId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

    }
}
