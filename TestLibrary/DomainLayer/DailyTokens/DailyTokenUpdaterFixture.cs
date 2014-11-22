using System;
using System.Collections.Generic;
using DomainLayer.DailyTokens;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using Moq;
using NUnit.Framework;

namespace TestLibrary.DomainLayer.DailyTokens
{
    [TestFixture]
    public class DailyTokenUpdaterFixture
    {
        private Mock<IAccountRepository> _acctRepo;
        private DailyTokenUpdater _updater;

        [SetUp]
        public void SetUp()
        {
            _acctRepo = new Mock<IAccountRepository>();
            _updater = new DailyTokenUpdater(_acctRepo.Object);
        }

        [Test]
        public void WillUpdateAcctsThatHaveNotBeenUpdated()
        {
            var acctInfo1 = new AccountInformation
                {
                    DailyToken = 123,
                    DateOfDailyToken = DateTime.Now.AddDays(-1)
                };
            var acctInfo2 = new AccountInformation
                {
                    DailyToken = 456,
                    DateOfDailyToken = DateTime.Now.Date
                };
            var accts = new List<AccountInformation> {acctInfo1, acctInfo2};
            _acctRepo.Setup(x => x.GetAllAccountInformations()).Returns(accts);

            _updater.Update();

            Assert.AreEqual(acctInfo1.DateOfDailyToken, DateTime.Now.Date);
            Assert.AreEqual(acctInfo2.DateOfDailyToken, DateTime.Now.Date);
            _acctRepo.Verify(x => x.SaveAccountInformation(acctInfo1));
            _acctRepo.Verify(x => x.SaveAccountInformation(acctInfo2), Times.Never);
        }

        [Test]
        public void WillUpdateTheDailyToken()
        {
            var acctInfo1 = new AccountInformation
            {
                DailyToken = 123,
                DateOfDailyToken = DateTime.Now.AddDays(-1)
            };
            var acctInfo2 = new AccountInformation
            {
                DailyToken = 456,
                DateOfDailyToken = DateTime.Now.Date
            };
            var accts = new List<AccountInformation> { acctInfo1, acctInfo2 };
            _acctRepo.Setup(x => x.GetAllAccountInformations()).Returns(accts);

            _updater.Update();

            Assert.AreNotEqual(acctInfo1.DailyToken, 123);
            Assert.AreEqual(acctInfo2.DailyToken, 456);
        }
    }
}