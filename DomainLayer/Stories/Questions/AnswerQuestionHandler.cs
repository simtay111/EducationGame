using System.Data;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories.Questions
{
    public class AnswerQuestionHandler
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IStoryToDoItemRepository _storyToDoItemRepository;
        private readonly IPointsWithCompanyRepository _pointsWithCompanyRepository;
        private readonly IMemberRepository _memberRepository;

        public AnswerQuestionHandler(IQuestionRepository questionRepository, IStoryToDoItemRepository storyToDoItemRepository, IPointsWithCompanyRepository pointsWithCompanyRepository, IMemberRepository memberRepository)
        {
            _questionRepository = questionRepository;
            _storyToDoItemRepository = storyToDoItemRepository;
            _pointsWithCompanyRepository = pointsWithCompanyRepository;
            _memberRepository = memberRepository;
        }

        public AnswerQuestionResponse Handle(AnswerQuestionRequest request)
        {
            var toDoItem = _storyToDoItemRepository.GetById(request.StoryToDoItemId);
            if (toDoItem == null)
                throw new AnswerQuestionException();
            var question = _questionRepository.GetById(toDoItem.ToDoId);
            if (question == null)
                throw new AnswerQuestionException();

            var acctInfo = question.Story.AccountInformation;
            var pointsForMember = _pointsWithCompanyRepository.GetForMemberForAcct(request.MemberId, acctInfo.Id);

            if (pointsForMember == null)
                pointsForMember = CreateNewPointsWithCompany(request, acctInfo);

            if (question.AnswerBool == request.Answer)
                pointsForMember.Points += 10;

            _pointsWithCompanyRepository.Save(pointsForMember);

            return new AnswerQuestionResponse();
        }

        private PointsWithCompany CreateNewPointsWithCompany(AnswerQuestionRequest request, AccountInformation acctInfo)
        {
            return new PointsWithCompany
            {
                AccountInformation = acctInfo,
                Member = _memberRepository.GetById(request.MemberId)
            };
        }
    }
}