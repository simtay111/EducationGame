
using System.Collections.Generic;

namespace DomainLayer.Entities
{
    public class Member
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int TotalPoints { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual int QuizToken { get; set; }
        public virtual bool Inactive { get; set; }
        public virtual AccountInformation AccountInformation { get; set; }
    }
}
