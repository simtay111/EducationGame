using System;
using System.Collections.Generic;
using System.Dynamic;
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
            dynamic response = new ExpandoObject();
            var failureResponse = new { noNextSlide = true };

            if (toDoItem == null)
                return failureResponse;

            response.type = toDoItem.Type;
            response.toDoId = toDoItem.ToDoId;
            response.stepId = toDoItem.Id;

            if (toDoItem.Type == ToDoType.Slide)
            {
                var slide = _slideRepository.GetById(toDoItem.ToDoId);
                if (slide == null)
                    return failureResponse;
                response.body = slide.Body;
                response.title = slide.Title;
                return response;
            }
            if (toDoItem.Type == ToDoType.Question)
            {
                var question = _questionRepository.GetById(toDoItem.ToDoId);
                if (question == null)
                    return failureResponse;

                response.question = question.Query;
                response.answer = question.AnswerBool;
                response.correctInfo = question.CorrectAnswer;
                response.incorrectInfo = question.WrongAnswer;
                return response;
            }

            return failureResponse;
        }
    }
}