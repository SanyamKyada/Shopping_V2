namespace Shopping.Models.ViewModels
{
    public class Vm_CartItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Guid UserId { get; set; }
        public int SKUId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal Price { get; set; }
    }
}
