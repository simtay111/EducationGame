using DomainLayer.Entities;
using DomainLayer.Reports;

namespace DomainLayer.Email
{
    public class ReceiptSender
    {
        private readonly EmailSender _emailSender;
        private readonly SummaryBuilder _summaryBuilder;

        public ReceiptSender(EmailSender emailSender, SummaryBuilder summaryBuilder)
        {
            _emailSender = emailSender;
            _summaryBuilder = summaryBuilder;
        }

        public void SendReceipt(string email, Receipt receipt, string cc)
        {
            var nameValuesOfSummary = _summaryBuilder.BuildSummary(receipt.AccountInformation.Id);
            var receiptHeader =
                "Thanks for using PracticeOwl.\n\nMost chiropractor's agree that education is the most important aspect of growing and maintaining a chiropractic business.\n\nPracticeOwl makes educating your patients, fun & effortless!\n\n";
            foreach (var summaryItem in nameValuesOfSummary)
            {
                receiptHeader += summaryItem.Name + " " + summaryItem.Value + "\n";
            }
            receiptHeader += "\n";
            if (receipt.Cost >= (decimal)0.5)
                _emailSender.SendEmail(email, receiptHeader + receipt.ReceiptText, "PracticeOwl Receipt", cc);
            receipt.Sent = true;
        }
    }
}