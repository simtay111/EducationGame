using System.Linq;
using DataLayer;
using DomainLayer.Entities.Stories;
using LinqToExcel;

namespace TestLibrary
{
    public class QuizImporter
    {
        private DefaultStoryRepository _storyRepo;

        public QuizImporter(IConnectionProvider connectionProvider)
        {
            _storyRepo = new DefaultStoryRepository(connectionProvider);

        }
        public void Import(IConnectionProvider connectionProvider)
        {
            var excel = new ExcelQueryFactory();
            excel.FileName = @"..\..\ChiroAppy.xlsx";

            excel.AddMapping<ExcelImport>(x => x.QuizTitle, "QUIZ TITLE");
            excel.AddMapping<ExcelImport>(x => x.QuizSubHeading, "QUIZ TITLE SUB HEADING");
            excel.AddMapping<ExcelImport>(x => x.Slide1, "Slide1");
            excel.AddMapping<ExcelImport>(x => x.Slide2, "Slide2");
            excel.AddMapping<ExcelImport>(x => x.Slide3, "Slide3");
            excel.AddMapping<ExcelImport>(x => x.Slide4, "Slide4");
            excel.AddMapping<ExcelImport>(x => x.Question1, "Q1");
            excel.AddMapping<ExcelImport>(x => x.Question1Answer, "Q1Answer");
            excel.AddMapping<ExcelImport>(x => x.Question1Correct, "Q1Correct");
            excel.AddMapping<ExcelImport>(x => x.Question1Wrong, "Q1Wrong");
            excel.AddMapping<ExcelImport>(x => x.Question2, "Q2");
            excel.AddMapping<ExcelImport>(x => x.Question2Answer, "Q2Answer");
            excel.AddMapping<ExcelImport>(x => x.Question2Correct, "Q2Correct");
            excel.AddMapping<ExcelImport>(x => x.Question2Wrong, "Q2Wrong");
            excel.AddMapping<ExcelImport>(x => x.Question3, "Q3");
            excel.AddMapping<ExcelImport>(x => x.Question3Answer, "Q3Answer");
            excel.AddMapping<ExcelImport>(x => x.Question3Correct, "Q3Correct");
            excel.AddMapping<ExcelImport>(x => x.Question3Wrong, "Q3Wrong");
            excel.AddMapping<ExcelImport>(x => x.References, "InfoReferences");
            excel.AddMapping<ExcelImport>(x => x.MessageLesson, "MessageLesson");
            excel.AddMapping<ExcelImport>(x => x.Order, "Order");
            var slideRepo = new DefaultSlideRepository(connectionProvider);
            var questionRepo = new DefaultQuestionRepository(connectionProvider);
            var allSlides = slideRepo.GetAll();
            var theStories = _storyRepo.GetAll();
            foreach (var allSlide in allSlides)
            {
                slideRepo.Delete(allSlide.Id);
            }

            var allQuestions = questionRepo.GetAll();
            foreach (var allSlide in allQuestions)
            {
                questionRepo.Delete(allSlide.Id);
            }
            foreach (var story in theStories)
            {
                _storyRepo.Delete(story.Id);
            }

            var record = from x in excel.Worksheet<ExcelImport>("Quizes") select x;
            foreach (var rec in record)
            {
                DefaultStory story = null;
                story = new DefaultStory
                {
                    Name = rec.QuizTitle,
                    Summary = rec.QuizSubHeading,
                    InfoReferences = rec.References,
                    StoryOrder = rec.Order,
                    MessageLessonText = rec.MessageLesson
                };
                _storyRepo.Save(story);


                var slide1 = new DefaultSlide
                {
                    Body = rec.Slide1,
                    Story = story,
                    Title = "Title"
                };
                var slide2 = new DefaultSlide
                {
                    Body = rec.Slide2,
                    Story = story,
                    Title = "Title"
                };
                var slide3 = new DefaultSlide
                {
                    Body = rec.Slide3,
                    Story = story,
                    Title = "Title"
                };
                var slide4 = new DefaultSlide
                {
                    Body = rec.Slide4,
                    Story = story,
                    Title = "Title"
                };

                new DefaultSlideRepository(connectionProvider).Save(slide1);
                new DefaultSlideRepository(connectionProvider).Save(slide2);
                new DefaultSlideRepository(connectionProvider).Save(slide3);
                new DefaultSlideRepository(connectionProvider).Save(slide4);
                var question1 = new DefaultQuestion
                {
                    Query = rec.Question1,
                    CorrectAnswer = rec.Question1Correct,
                    WrongAnswer = rec.Question1Wrong,
                    Story = story,
                    OrderToDisplay = 1,
                    AnswerBool = rec.Question1Answer
                };
                var question2 = new DefaultQuestion
                {
                    Query = rec.Question2,
                    CorrectAnswer = rec.Question2Correct,
                    WrongAnswer = rec.Question2Wrong,
                    Story = story,
                    OrderToDisplay = 2,
                    AnswerBool = rec.Question2Answer
                };
                var question3 = new DefaultQuestion
                {
                    Query = rec.Question3,
                    CorrectAnswer = rec.Question3Correct,
                    WrongAnswer = rec.Question3Wrong,
                    Story = story,
                    OrderToDisplay = 3,
                    AnswerBool = rec.Question3Answer
                };
                new DefaultQuestionRepository(connectionProvider).Save(question1);
                new DefaultQuestionRepository(connectionProvider).Save(question2);
                new DefaultQuestionRepository(connectionProvider).Save(question3);
            }
        }
    }
}
public class ExcelImport
{
    public string QuizTitle { get; set; }
    public string QuizSubHeading { get; set; }
    public string Slide1 { get; set; }
    public string Slide2 { get; set; }
    public string Slide3 { get; set; }
    public string Slide4 { get; set; }
    public string Question1 { get; set; }
    public bool Question1Answer { get; set; }
    public string Question1Correct { get; set; }
    public string Question1Wrong { get; set; }
    public string Question2 { get; set; }
    public bool Question2Answer { get; set; }
    public string Question2Correct { get; set; }
    public string Question2Wrong { get; set; }
    public string Question3 { get; set; }
    public bool Question3Answer { get; set; }
    public string Question3Correct { get; set; }
    public string Question3Wrong { get; set; }
    public string References { get; set; }
    public string UniqueStoryId { get; set; }
    public string MessageLesson { get; set; }
    public int Order { get; set; }
}