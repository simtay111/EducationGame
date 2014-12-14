namespace DomainLayer.Entities.Quizes
{
    public class PointsWithCompany
    {
        public int Id { get; set; }
        public AccountInformation AccountInformation { get; set; } 
        public Member Member { get; set; }
        public int Points { get; set; }
    }
}