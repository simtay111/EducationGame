using System;
using System.Collections.Generic;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using LinqToExcel.Extensions;

namespace DomainLayer.Stories.Responses
{
    public class StoryStepDataBuilder
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IQuestionRepository _questionRepository;

        public StoryStepDataBuilder(ISlideRepository slideRepository, IQuestionRepository questionRepository)
        {
            _slideRepository = slideRepository;
            _questionRepository = questionRepository;
        }

        public Object GenerateFromStoryToDoItem(StoryToDoItem toDoItem)
        {
            var response = new { };
            var failureResponse = new { noNextSlide = true };

            response.SetProperty("type", toDoItem.Type);
            response.SetProperty("toDoId", toDoItem.ToDoId);
            response.SetProperty("stepId", toDoItem.Id);

            if (toDoItem.Type == ToDoType.Slide)
            {
                var slide = _slideRepository.GetById(toDoItem.ToDoId);
                if (slide == null)
                    return failureResponse;
                response.SetProperty("body", slide.Body);
                response.SetProperty("title", slide.Title);
                return response;
            }
            if (toDoItem.Type == ToDoType.Slide)
            {
                var question = _questionRepository.GetById(toDoItem.ToDoId);
                if (question == null)
                    return failureResponse;
                response.SetProperty("question", question.Query);
                response.SetProperty("answer", question.AnswerBool);
                response.SetProperty("correctInfo", question.CorrectAnswer);
                response.SetProperty("incorrectInfo", question.WrongAnswer);
                return response;
            }

            return failureResponse;
        }
    }
}