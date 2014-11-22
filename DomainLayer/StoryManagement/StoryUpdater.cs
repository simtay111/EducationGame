using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.StoryManagement
{
    public class StoryUpdater
    {
        private readonly IStoryRepository _storyRepo;
        private readonly ISlideRepository _slideRepo;
        private readonly IQuestionRepository _questionRepo;

        public StoryUpdater(IStoryRepository storyRepo, ISlideRepository slideRepo, IQuestionRepository questionRepo)
        {
            _storyRepo = storyRepo;
            _slideRepo = slideRepo;
            _questionRepo = questionRepo;
        }

        public void Update(Story newStory, List<Slide> newSlides, List<Question> newQuestions, AccountInformation acctInfo)
        {
            if (newStory.Id < 1)
            {

                var storiesForAcct = _storyRepo.GetAllForAcctInfo(acctInfo.Id);
                newStory.StoryOrder = storiesForAcct.OrderByDescending(x => x.StoryOrder).First().StoryOrder + 1;
                newStory.AccountInformation = acctInfo;
                _storyRepo.Save(newStory);
                var questionOrder = 1;
                foreach (var question in newQuestions)
                {
                    question.OrderToDisplay = questionOrder;
                    question.Story = newStory;
                    questionOrder++;
                    _questionRepo.Save(question);
                }
                foreach (var slide in newSlides)
                {
                    slide.Story = newStory;
                    _slideRepo.Save(slide);
                }
            }
            else
            {
                var existingStory = _storyRepo.GetById(newStory.Id);
                var existingSlides = _slideRepo.GetForStory(existingStory.Id);
                var existingQuestions = _questionRepo.GetForStory(existingStory.Id);

                foreach (var slide in newSlides)
                {
                    var existingSlide = existingSlides.Single(x => x.Id == slide.Id);
                    existingSlide.Title = slide.Title;
                    existingSlide.Body = slide.Body;
                    _slideRepo.Save(existingSlide);
                }
                foreach (var question in newQuestions)
                {
                    var existingQuestion = existingQuestions.Single(x => x.Id == question.Id);
                    existingQuestion.AnswerBool = question.AnswerBool;
                    existingQuestion.CorrectAnswer = question.CorrectAnswer;
                    existingQuestion.Query = question.Query;
                    existingQuestion.WrongAnswer = question.WrongAnswer;
                    _questionRepo.Save(existingQuestion);
                }
                existingStory.MessageLessonText = newStory.MessageLessonText;
                existingStory.Name = newStory.Name;
                existingStory.Summary = newStory.Summary;
                _storyRepo.Save(existingStory);
            }
        }
    }
}