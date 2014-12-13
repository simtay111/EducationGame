using System.Collections.Generic;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class GetStoryHistoryForUser
    {
        private readonly IMemberQuizStatusRepository _quizStatusRepository;
        public GetStoryHistoryForUser(IMemberQuizStatusRepository quizStatusRepository)
        {
            _quizStatusRepository = quizStatusRepository;
        }

        public GetStoryHistoryResponse Handle(GetStoryHistoryRequest request)
        {
            var history = _quizStatusRepository.GetHistoryForMember(request.MemberId);

            var getStoryHistoryResponse = new GetStoryHistoryResponse {History = history};
            return getStoryHistoryResponse;
        }
    }

    public class GetStoryHistoryResponse
    {
        public List<MemberQuizStatus> History { get; set; }
    }

    public class GetStoryHistoryRequest
    {
        public int MemberId { get; set; }
    }
}