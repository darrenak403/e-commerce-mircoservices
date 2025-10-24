namespace OrderApi.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ClientId { get; set; }
        public int PurchaseQuantity { get; set; }
        public DateTime OrderedData { get; set; } = DateTime.UtcNow;
    }
}
