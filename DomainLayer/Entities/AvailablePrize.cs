namespace DomainLayer.Entities
{
    public class AvailablePrize : IPrize
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Points { get; set; }
        public long Cost { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRange { get; set; }
    }
}