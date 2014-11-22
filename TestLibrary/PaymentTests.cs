using System;
using NUnit.Framework;
using WePaySDK;

namespace TestLibrary
{
    [TestFixture]
    public class PaymentTests
    {

        [Test]
        public void Status()
        {
            
            var request = new CreditCardRequest();
            WePayConfig.productionMode = true;

            var response = new CreditCard().Post(request);

            Console.WriteLine(response.state);
        }
        [Test]
        public void FindForAccount()
        {

        }

        [Test]
        public void Authorize()
        {

            var request = new CreditCardAuthorizeRequest();
            WePayConfig.productionMode = true;

            var response = new CreditCard().Authorize(request);

            Console.WriteLine(response.state);
        }
        [Test]
        public void DoTest()
        {
            var request = new CheckoutCreateRequest();
            request.short_description = "Practice Owl New Patient";
            WePayConfig.productionMode = true;

            try
            {
                var response = new Checkout().Post(request);
            Console.WriteLine(response.Error);
            }
            catch (WePayException ex)
            {
                Console.WriteLine(ex.error_description);
                Console.WriteLine(ex.error);
                
            }
        }

    }
}