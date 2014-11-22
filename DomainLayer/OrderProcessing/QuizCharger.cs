using System;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.OrderProcessing
{
    public class QuizCharger
    {
        private readonly IReceiptRepository _receiptRepo;
        private readonly IMemberQuizStatusRepository _memberQuizStatusRepo;
        private readonly PaymentProcessor _paymentProcessor;

        public QuizCharger(IReceiptRepository receiptRepo, IMemberQuizStatusRepository memberQuizStatusRepo, PaymentProcessor paymentProcessor)
        {
            _receiptRepo = receiptRepo;
            _memberQuizStatusRepo = memberQuizStatusRepo;
            _paymentProcessor = paymentProcessor;
        }

        public void Charge()
        {
            var quizzesByAcct = _memberQuizStatusRepo.GetNonPayedForCompletedQuizzes().GroupBy(x => x.Member.AccountInformation);
            foreach (var acctQuizzes in quizzesByAcct)
            {
                var receiptString = string.Format("Games played this week: {0}", acctQuizzes.Count());
                var quizIds = "Games: ";
                var totalCost = acctQuizzes.Count() * (acctQuizzes.Key.CostPerQuiz);
                if (totalCost < 1)
                    continue;
                foreach (var quiz in acctQuizzes)
                {
                    quiz.DatePayedFor = DateTime.Now;
                    quiz.PayedFor = true;
                    quiz.PayedAmount = acctQuizzes.Key.CostPerQuiz;
                    quizIds += quiz.Id + ",";
                    _memberQuizStatusRepo.Save(quiz);
                }
                receiptString += "\nPracticeOwl Charges $" + totalCost;
                var receipt = new Receipt
                    {
                        AccountInformation = acctQuizzes.Key,
                        Cost = totalCost,
                        ReceiptText = receiptString,
                        DateBilled = DateTime.Now
                    };
                var audit = new PaymentAudit
                    {
                        AccountInformation = acctQuizzes.Key,
                        CreditCardToken = acctQuizzes.Key.CreditCardToken,
                        ItemIds = quizIds,
                        Amount = receipt.Cost
                    };
                _paymentProcessor.MakePayment(receipt.Cost, acctQuizzes.Key.CreditCardToken, audit);
                _receiptRepo.Save(receipt);
            }
        }
    }
}