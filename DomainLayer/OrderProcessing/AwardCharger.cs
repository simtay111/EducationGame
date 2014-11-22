using System;
using System.Linq;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.OrderProcessing
{
    public class AwardCharger
    {
        private readonly IAwardedPrizeRepository _awardsRepo;
        private readonly IReceiptRepository _receiptRepo;
        private readonly PaymentProcessor _paymentProcessor;

        public AwardCharger(IAwardedPrizeRepository awardsRepo, IReceiptRepository receiptRepo, PaymentProcessor paymentProcessor)
        {
            _awardsRepo = awardsRepo;
            _receiptRepo = receiptRepo;
            _paymentProcessor = paymentProcessor;
        }

        public void Charge()
        {
            var awardsByAcct = _awardsRepo.GetNonBilledRedeemedTangoAwards().GroupBy(x => x.Member.AccountInformation);
            foreach (var acctAwards in awardsByAcct)
            {
                var receiptString = "";
                var awardIds = "Awards: ";
                var totalPointCost = 0;
                foreach (var award in acctAwards)
                {
                    awardIds += award.Id + ",";
                    totalPointCost += award.PrizePoints;
                }
                receiptString += "\nNumber of rewards given out: " + acctAwards.Count();
                receiptString += "\nTotal: $" + totalPointCost / 100;
                var receipt = new Receipt
                    {
                        AccountInformation = acctAwards.Key,
                        Cost = totalPointCost / 100,
                        ReceiptText = receiptString,
                        DateBilled = DateTime.Now
                    };
                var audit = new PaymentAudit
                    {
                        AccountInformation = acctAwards.Key,
                        CreditCardToken = acctAwards.Key.CreditCardToken,
                        ItemIds = awardIds,
                        Amount = receipt.Cost
                    };
                var response = _paymentProcessor.MakePayment(receipt.Cost, acctAwards.Key.CreditCardToken, audit);
                if (response.checkout_id != 0)
                {
                    foreach (var award in acctAwards)
                    {
                        award.BilledToOffice = true;
                        award.DateBilledToOffice = DateTime.Now;
                        _awardsRepo.Save(award);
                    }
                    _receiptRepo.Save(receipt);
                }
            }
        }

        public void ChargeForSingleAward(AwardedPrize award)
        {
            var receiptString = award.PrizeName + " for " + award.Member.LastName + "," + award.Member.FirstName;

            receiptString += "\nTotal: $" + award.PrizePoints / 100;
            var receipt = new Receipt
            {
                AccountInformation = award.AccountInformation,
                Cost = award.PrizePoints / 100,
                ReceiptText = receiptString,
                DateBilled = DateTime.Now
            };
            var audit = new PaymentAudit
            {
                AccountInformation = award.AccountInformation,
                CreditCardToken = award.AccountInformation.CreditCardToken,
                ItemIds = award.Id.ToString(),
                Amount = receipt.Cost
            };
            var response = _paymentProcessor.MakePayment(receipt.Cost, award.AccountInformation.CreditCardToken, audit);
            if (response.checkout_id != 0)
            {
                award.DateBilledToOffice = DateTime.Now;
                award.BilledToOffice = true;
                _receiptRepo.Save(receipt);
            }
        }
    }
}