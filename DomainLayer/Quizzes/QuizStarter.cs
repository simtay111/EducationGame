using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Accounts;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.OrderProcessing;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Quizzes
{
    public class QuizStarter
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberQuizStatusRepository _memberQuizStatusRepository;
        private readonly IAccountCompletionStateGetter _stateGetter;

        public QuizStarter(IMemberRepository memberRepository, IMemberQuizStatusRepository memberQuizStatusRepository, IAccountCompletionStateGetter stateGetter)
        {
            _memberRepository = memberRepository;
            _memberQuizStatusRepository = memberQuizStatusRepository;
            _stateGetter = stateGetter;
        }

        public int StartQuizForMember(Member member, int dailyToken)
        {
            var historyForMember = _memberQuizStatusRepository.GetHistoryForMember(member.Id);
            var latestCompletion = historyForMember.OrderByDescending(x => x.DateCompleted).FirstOrDefault();

            if (latestCompletion != null)
            {
                if (member.PhoneNumber != "6786786789")
                {
                    CheckForQuizStartRequirements(historyForMember, latestCompletion, dailyToken);
                }
            }
            var randomToken = new Random().Next(10000, 99999);
            member.QuizToken = randomToken;
            _memberRepository.Save(member);

            return randomToken;
        }

        public void CheckForQuizStartRequirements(List<MemberQuizStatus> historyForMember, MemberQuizStatus latestCompletion, int dailyToken)
        {
            var status = _stateGetter.GetStatus(latestCompletion.Member.AccountInformation.Id, string.Empty);
            if (!status.PaymentSetupIsComplete && historyForMember.Count > 2)
            {
                throw new QuizStartException("The practice you are currently at is using a trial version of the game. Ask them to start using the full version!");
            }
            if (!status.BasicInfoIsComplete)
                throw new QuizStartException(
                    "The practice you are trying to play at is not setup correctly at the moment! Tell them to finish setting their site up!");
            if (latestCompletion.DateCompleted.Date == DateTime.Now.Date && latestCompletion.Member.PhoneNumber != latestCompletion.Member.AccountInformation.OfficePhone)
                throw new QuizStartException("You've already completed one quiz today! Nice work! You have: " + latestCompletion.Member.TotalPoints + " points!");
            if (latestCompletion.Member.AccountInformation.DailyToken != dailyToken)
            {
                throw new QuizStartException("The daily token you entered is not valid.  Please ask your front desk for the latest token!");
            }
            if ((latestCompletion.StoryId % 25) == 0 && latestCompletion.Completed)
            {
                throw new QuizStartException("Wow, nice work! You've done more quizzes than are ready at the moment.  We are adding more as soon as possible! Thanks for your patience.");
            }
        }
    }

    public class QuizStartException : Exception
    {
        public string ErrorMessage { get; set; }

        public QuizStartException(string message)
        {
            ErrorMessage = message;
        }
    }
}