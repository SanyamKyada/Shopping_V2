namespace Shopping.Models.DTO
{
    public class CartModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int SKUId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
