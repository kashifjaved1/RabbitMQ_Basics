namespace Producer.Data.Entities
{
    public class Order : DefaultEntity<int>
    {
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
