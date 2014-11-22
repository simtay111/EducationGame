using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AccountInformationMapping : ClassMap<AccountInformation>
    {
        public AccountInformationMapping()
        {
            Id(x => x.Id);
            Map(x => x.NotifyEmail1);
            Map(x => x.NotifyEmail2);
            Map(x => x.OfficeName);
            Map(x => x.OfficePhone);
            Map(x => x.PayedOnce);
            Map(x => x.NumberOfPatients);
            Map(x => x.NumberOfQuizesTaken);
            Map(x => x.IsVerified);
            Map(x => x.CreationDate);
            Map(x => x.SentDailyPrintout);
            Map(x => x.DailyToken);
            Map(x => x.DateOfDailyToken);
            Map(x => x.CreditCardToken);
            Map(x => x.Gpa);
            Map(x => x.LastAccountSignedOn);
            Map(x => x.CostPerQuiz);
            Map(x => x.DateOfPrintedDaily);
            Map(x => x.Autopay);
            Map(x => x.DatePayedThrough);
            Map(x => x.SubscriptionCost);
        }
    }
}