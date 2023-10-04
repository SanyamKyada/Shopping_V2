namespace Shopping.Models.DTO
{
    public class SKUAttributeModel
    {
        public int Id { get; set; }
        public int AttributeMasterId { get; set; }
        public string Value { get; set; }
        public int SKUId { get; set; }
        public SKUModel SKU { get; set; }
    }
}
