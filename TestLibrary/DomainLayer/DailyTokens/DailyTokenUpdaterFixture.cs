using System;
using System.Collections.Generic;
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

        [SetUp]
        public void SetUp()
        {
            _acctRepo = new Mock<IAccountRepository>();
        }

    }
}