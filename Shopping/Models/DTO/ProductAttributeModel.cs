namespace Shopping.Models.DTO
{
    public class ProductAttributeModel
    {
        public int Id { get; set; }
        public int AttributeMasterId { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
    }
}
