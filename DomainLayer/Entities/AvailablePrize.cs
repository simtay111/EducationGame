namespace DomainLayer.Entities
{
    public class AvailablePrize
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Points { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublic { get; set; }
    }
}