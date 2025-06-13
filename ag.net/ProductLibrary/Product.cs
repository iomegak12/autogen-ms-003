namespace ProductLibrary
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public int QuantityAvailable { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}