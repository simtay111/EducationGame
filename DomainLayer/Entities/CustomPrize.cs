
namespace DomainLayer.Entities
{
    public interface IPrize
    {
        int Id { get; set; }
        string Name { get; set; }
        int Points { get; set; }
        string Sku { get; set; }
        string ImageUrl { get; set; }
        bool IsRange { get; set; }
    }

    public class CustomPrize : IPrize
    {
        public virtual int Id { get; set; }
        public virtual AccountInformation AccountInformation { get; set; }
        public virtual string Name { get; set; }
        public virtual int Points { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRange { get; set; }
    }
}