using System;
using System.Linq;
using DomainLayer.ECards;
using NUnit.Framework;
using TangoApi;
using TangoApi.Entity;

namespace TestLibrary
{
    public class TangoTests
    {
        [SetUp]
        public void SetUp()
        {
            TangoCredentials.Identifier = "PracticeOwlTest";
            TangoCredentials.Key = "UXFnQvLunCciAOsiTQzgFkX9XGT9MbiKHKqAwbdvFhqc6MGXe8gewpTkA";
            TangoCredentials.Endpoint = "https://sandbox.tangocard.com/raas/v1";
        }
        private TangoAcctInfoProvider _infoProvider = new TangoAcctInfoProvider();
        [Test]
        public void FirstTest()
        {
            var fetcher = new AvailableItemFetcher(new ServiceProxy());
            fetcher.Fetch();
        }

        [Test]
        [Ignore]
        public void CreateAccount()
        {
            var request = new CreateAccountRequest
                {
                    email = "simtay111@gmail.com",
                    customer = _infoProvider.GetCustomer(),
                    identifier = _infoProvider.GetAcctInfo()
                };
            var accountCreator = new AccountCreator(new ServiceProxy());
            accountCreator.CreateAccount(request);
        }

        [Test]
        [Ignore]
        public void AddFunds()
        {
            var request = new AddFundsRequest
                {
                    customer = _infoProvider.GetCustomer(),
                    account_identifier = _infoProvider.GetAcctInfo(),
                    amount = 10000, //CENTS
                    client_ip = "208.100.173.42",
                    credit_card = new TangoCcInfo
                        {
                            billing_address = new TangoBillingInformation
                                {
                                    f_name = "Simon",
                                    l_name = "Taylor",
                                    address = "20202 Star Ridge Court",
                                    city = "Bend",
                                    state = "OR",
                                    country = "USA",
                                    email = "simtay111@gmail.com",
                                    zip = "97701"
                                },
                        }
                };

            var adder = new FundsAdder(new ServiceProxy());
            adder.AddFunds(request);
        }

        [Test]
        [Ignore]
        public void PlaceOrder()
        {
            var order = new OrderRequest
                {
                    customer = _infoProvider.GetCustomer(),
                    account_identifier = _infoProvider.GetAcctInfo(),
                    campaign= "",
                    sku = "TNGO-E-V-STD",
                    reward_message = "Hehe message",
                    reward_from = "Simtay222@gmail.com",
                    reward_subject = "haha subject",
                    recipient = new Recipient { email = "simtay111@gmail.com", name = "Simon" },
                    send_reward =  true
                };

            var placer = new OrderPlacer(new ServiceProxy());

            placer.Order(order.WithAmount(200));
        }

        [Test]
        public void GetAccountStatus()
        {
            var statusGetter = new AccountStatusFetcher(new ServiceProxy());

            var response = statusGetter.GetCurrentStatus(_infoProvider.GetCustomer(), _infoProvider.GetAcctInfo());

            Console.WriteLine(response.account.available_balance);
            Console.WriteLine(response.account.customer);
            Console.WriteLine(response.account.email);
            Console.WriteLine(response.account.identifier);
        }

        [Test]
        public void GetOrderHistory()
        {
            var statusGetter = new OrderHistoryFetcher(new ServiceProxy());
            var request = new OrderHistoryRequestData
                {
                    account_identifier = _infoProvider.GetAcctInfo(),
                    customer = _infoProvider.GetCustomer(),
                    offset = 0,
                    limit = 100,
                    start_date = DateTime.UtcNow.AddDays(-10),
                    end_date = DateTime.UtcNow,
                };

            var response = statusGetter.GetCurrentStatus(request);

            Console.WriteLine(response.orders.Last().amount);
        }
    }
}