using DomainLayer.Entities;
using DomainLayer.Reports;

namespace DomainLayer.Email
{
    public class ReceiptSender
    {
        private readonly EmailSender _emailSender;

        public ReceiptSender(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void SendReceipt(string email, Receipt receipt, string cc)
        {
            var receiptHeader =
                "Thanks for using PracticeOwl.\n\nMost chiropractor's agree that education is the most important aspect of growing and maintaining a chiropractic business.\n\nPracticeOwl makes educating your patients, fun & effortless!\n\n";
            receiptHeader += "\n";
            if (receipt.Cost >= (decimal)0.5)
                _emailSender.SendEmail(email, receiptHeader + receipt.ReceiptText, "PracticeOwl Receipt", cc);
            receipt.Sent = true;
        }
    }
}