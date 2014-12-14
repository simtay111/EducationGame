using System;
using System.Linq;
using System.Net.NetworkInformation;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;
using Remotion.Logging;

namespace DomainLayer.Stories
{
    public class FinishGameHandler
    {
        private readonly IMemberQuizStatusRepository _memberQuizStatusRepository;
        private readonly IStoryToDoItemRepository _storyToDoItemRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IPointsWithCompanyRepository _pointsWithCompanyRepository;

        public FinishGameHandler(IMemberQuizStatusRepository memberQuizStatusRepository, IStoryToDoItemRepository storyToDoItemRepository, IMemberRepository memberRepository, IStoryRepository storyRepository, IPointsWithCompanyRepository pointsWithCompanyRepository)
        {
            _memberQuizStatusRepository = memberQuizStatusRepository;
            _storyToDoItemRepository = storyToDoItemRepository;
            _memberRepository = memberRepository;
            _storyRepository = storyRepository;
            _pointsWithCompanyRepository = pointsWithCompanyRepository;
        }

        public FinishGameResponse Handle(FinishGameRequest request)
        {
            var member = _memberRepository.GetById(request.MemberId);
            if (member == null)
                throw new FinishGameException("No member");
            var storyToDoItems = _storyToDoItemRepository.GetByMemberId(request.MemberId);
            if (storyToDoItems.Any(x => x.Story.Id == request.GameId))
                throw new FinishGameException("You are not done with the game yet");

            var quizStatus = _memberQuizStatusRepository.GetByStoryIdAndMemberId(request.GameId, request.MemberId);
            if (quizStatus == null)
                throw new FinishGameException("You have not started this game yet");

            var story = _storyRepository.GetById(request.GameId);

            quizStatus.Completed = true;
            quizStatus.DateCompleted = DateTime.Now;
            _memberQuizStatusRepository.Save(quizStatus);


            var poitnsWithCompany = (_pointsWithCompanyRepository.GetForMemberForAcct(request.MemberId,
                story.AccountInformation.Id) ?? new PointsWithCompany()).Points;

            return new FinishGameResponse
            {
                AcctName = story.AccountInformation.CompanyName,
                StoryTitle = quizStatus.StoryName,
                PointsEarned = poitnsWithCompany
            };
        }
    }

    public class FinishGameException : Exception
    {
        public FinishGameException(string message) : base(message)
        {
        }
    }

    public class FinishGameRequest
    {
        public int MemberId { get; set; }
        public int GameId { get; set; }
    }

    public class FinishGameResponse
    {
        public string AcctName { get; set; }
        public string StoryTitle { get; set; }
        public int PointsEarned { get; set; }
    }
}