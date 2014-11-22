using System;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.OrderProcessing
{
    public class SubscriptionCharger
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IReceiptRepository _receiptRepo;
        private readonly PaymentProcessor _paymentProcessor;

        public SubscriptionCharger(IAccountRepository accountRepository, IReceiptRepository receiptRepo, PaymentProcessor paymentProcessor)
        {
            _accountRepository = accountRepository;
            _receiptRepo = receiptRepo;
            _paymentProcessor = paymentProcessor;
        }

        public void Charge(int acctInfoId, int chargeAmount)
        {
            TraceLog.WriteLine("SubscriptionCharger Charge " + acctInfoId);
            var acctInfo = _accountRepository.GetAcctInfoById(acctInfoId);


            var amountToCharge = (chargeAmount < 0) ? acctInfo.SubscriptionCost : chargeAmount;
            var numberOfMonthsToAdd = 1;
            if (amountToCharge == 141)
                numberOfMonthsToAdd = 3;
            if (amountToCharge == 997)
                numberOfMonthsToAdd = 12;
            

            var paymentAudit = new PaymentAudit
                {
                    AccountInformation = acctInfo,
                    Amount = amountToCharge,
                    CreditCardToken = acctInfo.CreditCardToken,
                    ItemIds = numberOfMonthsToAdd + " Month(s)"
                };
            var receipt = new Receipt
                {
                    AccountInformation = acctInfo,
                    Cost = amountToCharge,
                    DateBilled = DateTime.Now,
                    ReceiptText = numberOfMonthsToAdd + " Month(s) @ $" + amountToCharge
                };
            _receiptRepo.Save(receipt);
            if (amountToCharge > 0 && isNotAatA(acctInfoId) && isNot2ChirosMission(acctInfoId))
                _paymentProcessor.MakePayment(amountToCharge, acctInfo.CreditCardToken, paymentAudit);
            if (acctInfoId == 1 || acctInfoId == 4)
                paymentAudit.Status = PaymentStatus.Processed;

            if (paymentAudit.Status == PaymentStatus.Processed || acctInfo.SubscriptionCost == 0)
            {
                acctInfo.DatePayedThrough = DateTime.Now.AddMonths(numberOfMonthsToAdd);
                _accountRepository.SaveAccountInformation(acctInfo);
            }
        }

        private static bool isNotAatA(int acctInfoId)
        {
            return acctInfoId != 1;
        }

        private static bool isNot2ChirosMission(int acctInfoId)
        {
            return acctInfoId != 4;
        }
    }
}