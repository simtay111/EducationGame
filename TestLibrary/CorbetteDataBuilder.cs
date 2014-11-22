using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataLayer;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.Stories;
using NHibernate;
using NUnit.Framework;

namespace TestLibrary
{
    public class CorbetteDataBuilder
    {
        [Test]
        public void Build()
        {
            var connection = new MyTests.TestConnectionProvider().CreateConnection();

            CreateAccountInfo(connection);
            var accountRepo = new AccountRepository(new MyTests.TestConnectionProvider());
            var acctInfo = accountRepo.GetAcctInfoById(2);
            new NewAccountStoryAdder(new SlideRepository(new MyTests.TestConnectionProvider()),new StoryRepository(new MyTests.TestConnectionProvider()),new QuestionRepository(new MyTests.TestConnectionProvider()),new DefaultQuestionRepository(new MyTests.TestConnectionProvider()),new DefaultSlideRepository(new MyTests.TestConnectionProvider()), new DefaultStoryRepository(new MyTests.TestConnectionProvider())    ).AddStoriesToAccount(acctInfo);
            connection.Flush();
            CreateAccount(connection);
            var idsInNewDb = CreateMembersForCorbette(connection, acctInfo);
            var storiesForAcct = new StoryRepository(new MyTests.TestConnectionProvider()).GetAllForAcctInfo(acctInfo.Id).ToList().OrderBy(x => x.Id);

            WriteQuizStatuses(connection, storiesForAcct, idsInNewDb);
        }

        private static void WriteQuizStatuses(ISession connection, IOrderedEnumerable<Story> storiesForAcct, Dictionary<int, int> idsInNewDb)
        {
            using (StreamReader sr = new StreamReader(@"..\..\CorebetteData\trueMemberQuizStatus.csv"))
            {
                var line = sr.ReadLine();
                line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    Console.WriteLine(line);
                    var splitArray = line.Split(',');
                    if (idsInNewDb.ContainsKey(int.Parse(splitArray[9])))
                    {
                        var matchedId = idsInNewDb[int.Parse(splitArray[9])];
                        var storyId = storiesForAcct.ElementAt(int.Parse(splitArray[8].Replace("QUIZ", ""))).Id;
                        connection.CreateSQLQuery(string.Format(@"INSERT INTO MemberQuizStatus
           ([DateCompleted]
           ,[GeneratedToken]
           ,[PointsEarned]
           ,[StoryName]
           ,[PayedFor]
           ,[PayedAmount]
           ,[DatePayedFor]
           ,[Completed]
           ,[StoryId]
           ,[MemberId])
                         VALUES
                               ('{0}'
                               ,{1}
                               ,{2}
                               ,'{3}'
                               ,{4}
                               ,{5}
                               ,'{6}'
,{7},{8},{9})
                    ", splitArray[1], int.Parse(splitArray[2]), int.Parse(splitArray[3]), splitArray[4].Replace("'", ""),
                                                                1, 0.0, splitArray[1], int.Parse(splitArray[7]),
                                                                storyId, matchedId)).ExecuteUpdate();
                    connection.Flush();
                    }


                    line = sr.ReadLine();
                }
            }
        }

        private static Dictionary<int, int> CreateMembersForCorbette(ISession connection, AccountInformation acctInfo)
        {
            var newIds = new Dictionary<int, int>();
            using (StreamReader sr = new StreamReader(@"..\..\CorebetteData\memberQuizStatus.csv"))
            {
                var line = sr.ReadLine();
                line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    Console.WriteLine(line);
                    var splitArray = line.Split(',');
                    if (int.Parse(splitArray[7]) == 12)
                    {
                        var member = new Member()
                            {
                                AccountInformation = acctInfo,
                                FirstName = splitArray[1],
                                LastName = splitArray[2],
                                PhoneNumber = splitArray[3],
                                Inactive = int.Parse(splitArray[5]) == 1,
                                TotalPoints = 250,
                                QuizToken = int.Parse(splitArray[6])
                            };
                        new MemberRepository(new MyTests.TestConnectionProvider()).Save(member);
                        MyTests.TestConnectionProvider.Session.Flush();
                        newIds.Add(int.Parse(splitArray[0]), member.Id);
                    }
                    line = sr.ReadLine();
                }
            }

            return newIds;
        }

        private static void CreateAccount(ISession connection)
        {
            connection.CreateSQLQuery(@"
INSERT INTO Account
           ([DisplayName]
           ,[Email]
           ,[Password]
           ,[PasswordSalt]
           ,[PermissionLevel]
           ,[AccountInformationId])
     VALUES
           (NULL
           ,'CORBETTCHIRO@GMAIL.COM'
           ,'GMqiX4otC7TF/VJV7ng2TY7sTxAMk4sy6YLTIA+ozzI='
           ,'/gTIl9lY0u0='
           ,100
           ,2)



").ExecuteUpdate();
            connection.Flush();
        }

        private static void CreateAccountInfo(ISession connection)
        {
            connection.CreateSQLQuery(@"INSERT INTO AccountInformation
           ([NotifyEmail1]
           ,[NotifyEmail2]
           ,[OfficeName]
           ,[OfficePhone]
           ,[NumberOfPatients]
           ,[NumberOfQuizesTaken]
           ,[IsVerified]
           ,[CreationDate]
           ,[DailyToken]
           ,[DateOfDailyToken]
           ,[CreditCardToken]
           ,[Gpa]
           ,[CostPerQuiz])
     VALUES
           ('corbettchiro@gmail.com'
           ,NULL
           ,'Corbett Chiropractic & Health Enhancement'
           ,'5076458846'
           ,138
           ,249
           ,1
           ,'2014-03-25 10:10:54.000'
           ,0
           ,'2014-03-25 10:10:54.000'
           ,0
           ,1
           ,0.0)")
                      .ExecuteUpdate();

            connection.Flush();
        }
    }
}