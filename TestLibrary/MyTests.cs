using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataLayer;
using DomainLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.Stories;
using EducationGame.Controllers;
using NHibernate.Linq;
using NUnit.Framework;
using TangoApi;
using TangoApi.Entity;
using ISession = NHibernate.ISession;

namespace TestLibrary
{
    [TestFixture]
    public class MyTests
    {
        public class TestConnectionProvider : IConnectionProvider
        {
            public static ISession Session { get; set; }
            public ISession CreateConnection()
            {
                if (Session == null)
                    Session = TestHelper.OpenSession();
                return Session;
            }
        }

        [Test]
        public void CreatseDb()
        {
            TangoCredentials.Identifier = "PracticeOwlTest";
            TangoCredentials.Key = "UXFnQvLunCciAOsiTQzgFkX9XGT9MbiKHKqAwbdvFhqc6MGXe8gewpTkA";
            TangoCredentials.Endpoint = "https://sandbox.tangocard.com/raas/v1";
            CreateStories();
            TestConnectionProvider.Session.Flush();
            CreateMembers();
            TestConnectionProvider.Session.Flush();
            CreateMemberHistory();
            TestConnectionProvider.Session.Flush();
            CreatePrizes();
            TestConnectionProvider.Session.Flush();
            new DisclaimerBuilder().BuildDescriptionAndSkuData();
            TestConnectionProvider.Session.Flush();


            AddTangoPrizes();

            new TestConnectionProvider().CreateConnection()
                                        .CreateSQLQuery("update member set totalpoints = 5000")
                                        .ExecuteUpdate();
            new TestConnectionProvider().CreateConnection()
                                        .CreateSQLQuery("update accountInformation set creditcardtoken = 1690002520")
                                        .ExecuteUpdate();

            //new CorbetteDataBuilder().Build();
            TestConnectionProvider.Session.Flush();
        }

        private static void AddTangoPrizes()
        {
            var desiredSkus = new List<string>
                {
                    "AMZN-E-V-STD",
                    "APPLBS-E-500-STD",
                    "BSTB-E-500-STD",
                    "FACE-E-V-STD",
                    "FAND-E-500-STD",
                    "REII-E-500-STD",
                    "RESTDOTCOM-E-2500-STD",
                    "SEPH-E-V-STD",
                    "TRGT-E-500-BULS",
                    "XBOX-E-500-STD",
                    "TNGO-E-V-STD"
                };
            var fetcher = new AvailableItemFetcher(new ServiceProxy());
            var items = fetcher.Fetch().Where(x => x.rewards.SingleOrDefault(y => desiredSkus.Contains(y.sku)) != null);
            foreach (var item in items)
            {
                var request = new AddRewardRequest();
                request.Item = new AvailableItem();
                request.Item.description = item.description;
                request.Item.image_url = item.image_url;
                request.Reward = item.rewards.Single(x => desiredSkus.Contains(x.sku));


                var prizeRepo = new PrizeRepository(new TestConnectionProvider());
                var prize = new AvailablePrize
                    {
                        ImageUrl = request.Item.image_url,
                        Cost = (request.Reward.unit_price > 0) ? (int) request.Reward.unit_price : 500,
                        Name = request.Item.description,
                        Points = (request.Reward.unit_price > 0) ? (int) request.Reward.unit_price : 500,
                        Sku = request.Reward.sku,
                        IsRange = request.Reward.unit_price == -1
                    };
                prizeRepo.Save(prize);
                TestConnectionProvider.Session.Flush();
            }
        }


        public List<String> GetNames()
        {
            var reader = new StreamReader(File.OpenRead(@"../../CSV_Database_of_First_Names.csv"));
            List<string> listA = new List<string>();
            while (!reader.EndOfStream)
            {
                listA.Add(reader.ReadLine());
            }
            return listA;
        }

        public void CreateMembers()
        {
            var registerHandler = new CreateUserRequestHandler(new EmailSender(new AuditLogRepository(new TestConnectionProvider())), new AccountRepository(new TestConnectionProvider()), new PrizeRepository(new TestConnectionProvider()),
                                                               new PasswordHasher(), new NewAccountStoryAdder(new SlideRepository(new TestConnectionProvider()), new StoryRepository(new TestConnectionProvider()), new QuestionRepository(new TestConnectionProvider()), new DefaultQuestionRepository(new TestConnectionProvider()), new DefaultSlideRepository(new TestConnectionProvider()), new DefaultStoryRepository(new TestConnectionProvider())));

            registerHandler.Handle(new CreateUserRequest
                                       {
                                           UserName = "a@a.com",
                                           Password = "password",
                                           ConfirmPass = "password",
                                           PermissionLevel = RolesStatic.LlcAdmin
                                       });


            var connection = new TestConnectionProvider().CreateConnection();

            var accounts = (from acct in connection.Query<Account>() select acct).ToList();

            var names = GetNames();

            foreach (var account in accounts)
            {

                var random = new Random();
                var members = new List<Member>();
                for (int i = 0; i < 1; i++)
                {
                    var newIndex = random.Next(names.Count);
                    var lastName = random.Next(names.Count);
                    Console.WriteLine(names[newIndex]);
                    var member = new Member
                                     {
                                         FirstName = names[newIndex],
                                         LastName = names[lastName],
                                         PhoneNumber = "5413909003",
                                         TotalPoints = 30,
                                         AccountInformation = account.AccountInformation
                                     };

                    members.Add(member);
                }
                foreach (var mem in members)
                    connection.Save(mem);
            }
        }

        public void CreateMemberHistory()
        {
            var connection = new TestConnectionProvider().CreateConnection();

            var allMembers = (from acct in connection.Query<Member>() select acct).ToList();

            var random = new Random();
            var quizes = new List<MemberQuizStatus>();
            foreach (var member in allMembers)
            {
                for (int i = 0; i < 0; i++)
                {
                    var quizHistory = new MemberQuizStatus
                                          {
                                              DateCompleted = DateTime.Now.AddDays(random.Next(0, 5) * -1),
                                              GeneratedToken = 1234,
                                              Member = member,
                                              PointsEarned = random.Next(0, 100),
                                              StoryName = "Lesson on back surgery",
                                              Completed = true
                                          };
                    quizes.Add(quizHistory);
                }
            }
            foreach (var quiz in quizes)
            {
                connection.Save(quiz);
            }
        }
        public void CreatePrizes()
        {
            var connection = new TestConnectionProvider().CreateConnection();

            var allAccts = (from acct in connection.Query<AccountInformation>() select acct).ToList();
            var infoRepo = new AccountRepository(new TestConnectionProvider());
            foreach (var acct in allAccts)
            {
                acct.NotifyEmail1 = "nevinrosenberg@gmail.com";
                acct.OfficePhone = "5413909003";
                acct.OfficeName = "Simon's Office";

                infoRepo.SaveAccountInformation(acct);
            }
        }

        [Test]
        [Ignore]
        public void CreateStories()
        {
            var quizImporter = new QuizImporter(new TestConnectionProvider());
            quizImporter.Import(new TestConnectionProvider());
        }

        [Test]
        [Ignore]
        public void CanCreateDateThings()
        {
            var memberQuiz1 = new MemberQuizStatus { DateCompleted = DateTime.Now.AddDays(-3), Id = 5 };
            var memberQuiz2 = new MemberQuizStatus { DateCompleted = DateTime.Now.AddDays(3), Id = 7 };
            var memberQuiz3 = new MemberQuizStatus { DateCompleted = DateTime.Now.AddDays(0), Id = 9 };
            var status = new List<MemberQuizStatus> { memberQuiz1, memberQuiz2, memberQuiz3 }.OrderByDescending(x => x.DateCompleted);
            Console.WriteLine(DateTime.Now.AddMinutes(34).Date);

            Assert.AreEqual(DateTime.Now.AddMinutes(34).Date, DateTime.Now.Date);
        }

        //[Test]
        //[Ignore]
        //public void MigratePrizeInfo()
        //{
        //    var connection = new TestConnectionProvider();

        //    var accountInformationRepo = new AccountRepository(connection);
        //    var acctInfo = accountInformationRepo.GetAllAccountInformations();
        //    var prizeRepo = new PrizeRepository(connection);
        //    var memberRepo = new MemberRepository(connection);
        //    foreach (var acctInf in acctInfo)
        //    {
        //        try
        //        {
        //            var prizes = prizeRepo.GetForAccount(acctInf.Id).OrderBy(x => x.Points).ToList();
        //            var prizeSnapshot = new PrizeSnapshot
        //                {
        //                    Prize1 = prizes[0].Name,
        //                    Prize2 = prizes[1].Name,
        //                    Prize3 = prizes[2].Name,
        //                    Prize4 = prizes.Count > 3 ? prizes[3].Name : null,
        //                    Prize1Points = prizes[0].Points,
        //                    Prize2Points = prizes[1].Points,
        //                    Prize3Points = prizes[2].Points,
        //                    Prize4Points = prizes.Count > 3 ? prizes[3].Points : 0
        //                };
        //            prizeRepo.SaveSnapshot(prizeSnapshot);

        //            var members = memberRepo.GetMembersByAccountInfo(acctInf.Id, true);
        //            foreach (var mem in members)
        //            {
        //                mem.PrizeSnapshot = prizeSnapshot;
        //                memberRepo.Save(mem);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //           continue; 
        //        }

        //    }
        //}
        [Test]
        [Ignore]
        public void TestEmail()
        {
            var emailSender = new EmailSender(new AuditLogRepository(new TestConnectionProvider()));
            emailSender.SendEmail("simtay111@gmail.com", "Meow", "A subject");
        }
    }
}