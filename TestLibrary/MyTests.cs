using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.Entities.Stories;
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
            //TangoCredentials.Identifier = "PracticeOwlTest";
            //TangoCredentials.Key = "UXFnQvLunCciAOsiTQzgFkX9XGT9MbiKHKqAwbdvFhqc6MGXe8gewpTkA";
            //TangoCredentials.Endpoint = "https://sandbox.tangocard.com/raas/v1";
            CreateMembers();
            CreateSampleStory();
            TestConnectionProvider.Session.Flush();
        }

        private void CreateSampleStory()
        {
            var storyRepo = new StoryRepository(new TestConnectionProvider());
            var slideRepo = new SlideRepository(new TestConnectionProvider());
            var questionRepo = new QuestionRepository(new TestConnectionProvider());
            var accountRepo = new AccountRepository(new TestConnectionProvider());
            var accountInformation = new AccountInformation { Autopay = false, CompanyName = "Test Company", CreationDate = DateTime.Now, CreditCardToken = 0, DatePayedThrough = DateTime.Now, PayedOnce = true, SubscriptionCost = 99 };
            accountRepo.SaveAccountInformation(accountInformation);
            var story = new Story
            {
                AccountInformation = accountInformation, MessageLessonText = "A new lesson starts here",
                Name = "New Lesson",
                IsPublic = true,
                Summary = "This is a summary"
            };
            storyRepo.Save(story);
            slideRepo.Save(new Slide
            {
                Story = story,
                Body = "Slide 1 body",
                Title = "Slide 1 Title"
            });
            slideRepo.Save(new Slide
            {
                Story = story,
                Body = "Slide 2 body",
                Title = "Slide 2 Title"
            });
            questionRepo.Save(new Question
            {
                AnswerBool = true,
                CorrectAnswer = "correct answer 1",
                WrongAnswer =  "wrong answer 1",
                Query = "Query bro 1"
            });
            questionRepo.Save(new Question
            {
                AnswerBool = true,
                CorrectAnswer = "correct answer 2",
                WrongAnswer =  "wrong answer 2",
                Query = "Query bro 2"
            });
        }

        private static void AddTangoPrizes()
        {
            var desiredSkus = new List<string>
                {
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
                        Name = request.Item.description,
                        Points = 500,
                        Sku = request.Reward.sku,
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
            var registerHandler =
                new CreateAccountRequestHandler(new AccountRepository(new TestConnectionProvider()),
                    new PasswordHasher(), new EmailSender(new AuditLogRepository(new TestConnectionProvider())));

            registerHandler.Handle(new CreateUserRequest
                                       {
                                           UserName = "a@a.com",
                                           Password = "password",
                                           ConfirmPass = "password",
                                           PermissionLevel = RolesStatic.LlcAdmin
                                       });


            var memberCreationHandler = new CreateMemberRequestHandler(new MemberRepository(new TestConnectionProvider()), new PasswordHasher(),
                new EmailSender(new AuditLogRepository(new TestConnectionProvider())));

            memberCreationHandler.Handle(new CreateUserRequest
            {
                ConfirmPass = "password",
                Password = "password",
                PermissionLevel = 100,
                UserName = "member@member.com"
            });

        }
    }
}