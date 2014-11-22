using System;
using Twilio;

namespace DomainLayer.Email
{
    public interface IMessageSender
    {
        void SendMessage(string receipientAddress, string message, string breakWord);
    }

    public class SmsMessageSender : IMessageSender
    {
        public void SendMessage(string receipientAddress, string message, string breakWord)
        {
            string AccountSid = "AC38df21a5fcefce74f39c58b02c49bdf1";
            string AuthToken = "18fd7734146a2ce5874dd04359fdf5e0";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var firstPart = message;
            var secondPart = string.Empty;
            if (message.Length > 150)
            {
                var indexOfBreakWork = firstPart.IndexOf(breakWord);
                firstPart = ("1/2 " + message.Substring(0, indexOfBreakWork));
                secondPart = ("2/2 " + message.Substring(indexOfBreakWork));
            }
            SMSMessage result = null;

            result = twilio.SendSmsMessage("+15415267966", receipientAddress, firstPart, "");
            if (!string.IsNullOrEmpty(secondPart))
            {
                twilio.SendSmsMessage("+15415267966", receipientAddress, secondPart, "");
            }
            if (result.RestException != null)
                TraceLog.WriteLine(result.RestException.Message);
        }
    }
}