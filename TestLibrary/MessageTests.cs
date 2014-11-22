using DomainLayer.Email;
using NUnit.Framework;

namespace TestLibrary
{
    public class MessageTests
    {
        [Test]
        //[Ignore]
        public void SendMessage()
        {

            var sender = new SmsMessageSender();
            var msg =
                string.Format(
                    "Hello {0}, {1} {2} was just in our office lots and lots of content to overwhelm you and learned about {3} and thought of you! Here's a coupon for '{4}'",
                    "Billy Bob", "Simon", "Taylor", "benefits of milk", "50% off chiro care.");

            sender.SendMessage("5413909003", msg, "Here's");
        }
    }
}