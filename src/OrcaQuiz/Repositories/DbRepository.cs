using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models;
using OrcaQuiz.Models.Enums;
using OrcaQuiz.Utils;
using OrcaQuiz.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;

namespace OrcaQuiz.Repositories
{
    public class DbRepository : IOrcaQuizRepository
    {
        OrcaQuizContext context;
        IdentityDbContext identityContext;
        public DbRepository(OrcaQuizContext context,
            IdentityDbContext identityContext)
        {
            this.identityContext = identityContext;
            this.context = context;
        }
        public void CopyQuestionToTest(int questionId, int testId, string userName)
        {
            //var test = context.Tests.Single(o => o.Id == testId);
            var question = context.Questions
                .Include(o => o.Answers)
                .Single(o => o.Id == questionId);

            //var user = context.Users.Single(o => o.UserId == identityContext.Users
            //    .Single(iu => iu.UserName == userName).Id);

            var currentUser = identityContext.Users.Single(o => o.UserName == userName);
            var user = context.Users.Single(o => o.UserId == currentUser.Id);

            var copiedQuestion = new Question()
            {
                //Duplicate original question
                //Name = thisQuestion.Name,
                QuestionType = question.QuestionType,
                Tags = question.Tags,
                QuestionText = question.QuestionText,
                HasComment = question.HasComment,

                //Add specific properties
                SortOrder = question.SortOrder,
                CreatedDate = DateTime.UtcNow,
                Author = $"{user.FirstName} {user.Lastname}",

                //Question belongs to this test
                TestId = testId

            };
            context.Questions.Add(copiedQuestion);
            context.SaveChanges();

            foreach (var answer in question.Answers)
            {
                context.Answers.Add(new Answer
                {
                    AnswerText = answer.AnswerText,
                    IsCorrect = answer.IsCorrect,
                    QuestionId = copiedQuestion.Id,
                    SortOrder = answer.SortOrder
                });
            }
            context.SaveChanges();

        }

        public void CopyTestToModule(int testId, int moduleId)
        {
            throw new NotImplementedException();
        }

        public int CreateAnswer(int questionId)
        {
            var answers = context.Answers.Where(o => o.QuestionId == questionId).ToArray();
            var answer = new Answer()
            {
                AnswerText = "New answer...",
                SortOrder = answers.Length > 0 ? answers.Max(o => o.SortOrder) + 10 : 10,
                QuestionId = questionId
            };

            context.Answers.Add(answer);
            context.SaveChanges();
            //GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
            //    .Answers.Add(answer);

            return answer.Id;
        }

        public int CreateAnswer(int questionId, AnswerDetailVM viewModel)
        {
            throw new NotImplementedException();
        }

        public int CreateTest(Test test)
        {
            test.AuthorId = context.Users.Single(o => o.Email == test.Tags).Id;
            context.Add(test);
            context.SaveChanges();

            return context.Tests.Last().Id;
        }

        public int CreateTestQuestion(int testId)
        {
            var newQuestion = new Question()
            {
                TestId = testId,
                CreatedDate = DateTime.UtcNow,
            };

            context.Tests.SingleOrDefault(o => o.Id == testId).Questions.Add(newQuestion);
            //_tests.SingleOrDefault(o => o.Id == testId).Questions.Add(newQuestion);
            context.SaveChanges();

            return newQuestion.Id;
        }

        public Answer[] GetAllAnswers()
        {
            throw new NotImplementedException();
        }

        public List<Module> GetAllModules()
        {
            return context.Modules.OrderBy(o => o.Name).ToList();
        }

        public Question[] GetAllQuestions()
        {
            throw new NotImplementedException();
        }

        public Test[] GetAllTests()
        {
            return context.Tests.ToArray();
        }

        public object GetAllTestsImportData(int currentTestId)
        {
            var allTests = context.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .Where(o => o.Id != currentTestId)
                .ToArray();

            var allTestsData = allTests.Select(o => new
            {
                text = o.Name,
                children = o.Questions.Select(q => new
                {
                    id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                    text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                    children = q.Answers.Select(a => new
                    {
                        text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                        state = new { disabled = true }
                    })
                }),
            }).ToArray();
            return allTestsData;
        }

        public PdfSymbols GetCertificateSymbols(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public object GetCurrentTestImportData(int id)
        {
            var currentTest = context.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .Single(o => o.Id == id);

            var currentTestData = new
            {
                text = currentTest.Name,
                children = currentTest.Questions.Select(q => new
                {
                    id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                    text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                    children = q.Answers.Select(a => new
                    {
                        text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                        state = new { disabled = true }
                    })
                }),
            };
            return currentTestData;
        }

        public EditQuestionVM GetEditQuestionVM(int testId, int questionId)
        {
            var thisQuestion = context.Questions.SingleOrDefault(o => o.Id == questionId);
            var testQuestions = context.Questions.Where(o => o.TestId == testId).ToArray();

            var viewModel = new EditQuestionVM()
            {
                ItemType = new SelectListItem[]
                {
                    new SelectListItem { Value = ((int)QuestionType.SingleChoice).ToString(), Text="Single Choice"},
                    new SelectListItem { Value = ((int)QuestionType.MultipleChoice).ToString(), Text="Multiple Choice"},
                    new SelectListItem { Value = ((int)QuestionType.TextSingleLine).ToString(), Text="Single Line Text"},
                    new SelectListItem { Value = ((int)QuestionType.TextMultiLine).ToString(), Text="Multi Line Text"}
                },
                TestId = testId,
                QuestionText = thisQuestion.QuestionText,
                QuestionId = questionId,
                Type = thisQuestion.QuestionType,
                SortOrder = testQuestions.Count() > 0 ? testQuestions.Max(o => o.SortOrder) + 10 : 10,
                QuestionFormVM = new QuestionFormVM()
                {
                    QuestionText = thisQuestion.QuestionText,
                    QuestionType = thisQuestion.QuestionType,
                    SortOrder = thisQuestion.SortOrder,
                    HasComment = thisQuestion.HasComment,
                    IsInEditQuestion = true
                },
                HasComment = thisQuestion.HasComment,
            };

            if ((viewModel.Type == QuestionType.MultipleChoice) || (viewModel.Type == QuestionType.SingleChoice))
            {
                viewModel.AnswerDetailVMs = thisQuestion.Answers
                    .OrderBy(o => o.SortOrder)
                    .Select(o => new AnswerDetailVM()
                    {
                        AnswerId = o.Id,
                        AnswerText = o.AnswerText,
                        IsChecked = o.IsCorrect,
                        ShowAsCorrect = o.IsCorrect,
                        QuestionType = thisQuestion.QuestionType,
                        SortOrder = o.SortOrder
                    }).ToArray();
            }
            return (viewModel);
        }

        public ManageModuleTestsVM GetManageModuleTestVM(int moduleId)
        {
            throw new NotImplementedException();
        }

        public ManageTestQuestionsVM GetManageTestQuestionVM(int testId)
        {
            var test = context.Tests
                .Include(o => o.Questions)
                .Single(o => o.Id == testId);

            return new ManageTestQuestionsVM
            {
                TestId = test.Id,
                TestName = test.Name,
                Description = test.Description,
                Questions = test.Questions.OrderBy(o => o.SortOrder).ToList()
            };
        }

        public Module GetModuleById(int Id)
        {
            throw new NotImplementedException();
        }

        public QuestionFormVM GetPreviewQuestion(int questionId)
        {
            throw new NotImplementedException();
        }

        public QuestionFormVM GetPreviewQuestionPartial(int questionId)
        {
            var currentQuestion = context.Questions
                .Include(q => q.Answers)
                .Single(o => o.Id == questionId);

            var viewModel = new QuestionFormVM()
            {
                IsInTestSession = false,
                Answers = currentQuestion.Answers.Select(o => new AnswerDetailVM()
                {
                    AnswerId = o.Id,
                    AnswerText = o.AnswerText,
                    ShowAsCorrect = o.IsCorrect,
                    IsChecked = o.IsCorrect
                }).ToList(),
                QuestionText = currentQuestion.QuestionText,
                HasComment = currentQuestion.HasComment,
                QuestionType = currentQuestion.QuestionType
            };

            return viewModel;
        }

        public SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason)
        {
            throw new NotImplementedException();
        }

        public SessionIndexVM GetSessionIndexVM(int testId, string userName)
        {
            var currentTest = context.Tests
                .Include(t => t.Questions)
                .Single(o => o.Id == testId);

            var user = GetUser(userName);

            var viewModel = new SessionIndexVM()
            {
                UserId = user.Id,
                TestId = currentTest.Id,
                NumberOfQuestions = currentTest.Questions.Count(),
                TestDescription = currentTest.Description,
                TestName = currentTest.Name,
                TimeLimit = currentTest.TimeLimitInMinutes
            };

            return viewModel;
        }

        public ShowResultsVM GetShowResultsVM(int testId)
        {
            var test = context.Tests
                .Include(t => t.TestSessions)
                .Single(o => o.Id == testId);

            int maxScore = test.Questions.Count();
            double testPassPercentage = (double)test.PassPercentage / 100;
            var result = new
            {
                resultData = new
                {
                    maxScore = maxScore,
                    passPercentage = test.PassPercentage,
                    passResult = maxScore * testPassPercentage
                },
                students = test.TestSessions
                .Select(ts => new
                {
                    name = ts.User.FirstName + " " + ts.User.Lastname,
                    email = ts.User.Email,
                    testscore = TestSessionUtils.GetScore(ts, context.Answers.ToArray(), context.Questions.ToArray())
                }).ToArray()
            };
            return new ShowResultsVM
            {
                ResultDataJSON = JsonConvert.SerializeObject(result)
            };
        }

        public TestSession GetTestSessionById(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            var currentTestSession = context.TestSessions
                .Include(ts => ts.QuestionResults)
                .Single(o => o.Id == testSessionId);
            var currentTest = context.Tests
                .Include(t => t.Questions)
                .Single(o => o.Id == currentTestSession.TestId);
            var currentQuestion = currentTest.Questions.OrderBy(o => o.SortOrder).ElementAt(questionIndex - 1);
            var currentQuestionResult = currentTestSession.QuestionResults.SingleOrDefault(o => o.QuestionId == currentQuestion.Id);

            //var timeLeft = thisTest.TimeLimit - (DateTime.UtcNow - thisTestSession.StartTime);

            //var secondsLeft = TimeUtils.GetSecondsLeft(thisTest.TimeLimitInMinutes, thisTestSession.StartTime);
            var selectedAnswers = currentQuestionResult?.SelectedAnswers.Split(',');

            return new ViewQuestionVM()
            {
                TestId = currentTest.Id,
                TestTitle = currentTest.Name,
                NumOfQuestion = currentTest.Questions.Count(),
                QuestionIndex = questionIndex,
                SecondsLeft = currentTestSession.SecondsLeft,

                QuestionFormVM = new QuestionFormVM()
                {
                    IsInTestSession = isInSession,
                    QuestionType = currentQuestion.QuestionType,
                    QuestionText = currentQuestion.QuestionText,
                    HasComment = currentQuestion.HasComment,
                    Comment = currentQuestionResult?.Comment,
                    SelectedAnswers = selectedAnswers,
                    Answers = currentQuestion.Answers.Select(o => new AnswerDetailVM()
                    {
                        AnswerId = o.Id,
                        AnswerText = o.AnswerText,
                        ShowAsCorrect = ((!isInSession) && (o.IsCorrect)),
                        IsChecked = selectedAnswers == null ? false :
                            ((isInSession) && (selectedAnswers.Contains(o.Id.ToString()))),

                    }).ToList()
                }
            };
        }

        public void RemoveAnswerFromQuestion(int testId, int questionId, int answerId)
        {
            //var question = context.Tests.Single(o => o.Id == testId).Questions.Single(q => q.Id == questionId);
            //question.Answers.RemoveAll(a => a.Id == answerId);
            var answer = context.Answers.Single(o => o.Id == answerId);
            context.Answers.Remove(answer);
            context.SaveChanges();
        }

        public void RemoveQuestionFromTest(int questionId, int testId)
        {
            var test = context.Tests.Single(o => o.Id == testId);
            var question = test.Questions.Single(o => o.Id == questionId);
            var answers = context.Answers.Where(o => o.QuestionId == questionId).ToArray();

            foreach (var answer in answers)
                context.Answers.Remove(answer);

            context.SaveChanges();
            //test.Questions.RemoveAll(o => o.Id == questionId);
            context.Questions.Remove(question);
            context.SaveChanges();
        }

        public void RemoveTestFromModule(int testId, int moduleId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> StartNewSession(string userName, int testId)
        {
            var currentUser = GetUser(userName);
            var currentTest = context.Tests
                .Include(t => t.Questions)
                .Single(o => o.Id == testId);
            var currentTestSession = new TestSession();

            context.TestSessions.Add(new TestSession
            {
                StartTime = DateTime.UtcNow,
                SecondsLeft = currentTest.TimeLimitInMinutes * 60,
                QuestionResults = new List<QuestionResult>(),
                TestId = currentTest.Id,
                UserId = currentUser.Id
            });
            var result = await context.SaveChangesAsync();

            //currentUser.TestSessions.Add(new TestSession()
            //{
            //    Id = _testSessions.Count() + 1,
            //    QuestionResults = new List<QuestionResult>(),
            //    StartTime = DateTime.UtcNow,
            //    SecondsLeft = currentTest.TimeLimitInMinutes * 60,
            //    TestId = testId,
            //    UserId = userId,
            //});
            //if (result > 0)
            //{
            //    for (int i = 0; i < currentTest.Questions.Count(); i++)
            //    {
            //        context.QuestionResults.Add(new QuestionResult
            //        {
            //            QuestionId = currentTest.Questions[i].Id,
            //            SelectedAnswers = "",
            //        });
            //    }
            //    await context.SaveChangesAsync();

            //}
            //for (int i = 1; i <= currentTest.Questions.Count(); i++)
            //    currentUser.TestSessions.Last().QuestionResults.Add(new QuestionResult()
            //    {
            //        Id = _questionResults.Count() + i,
            //        QuestionId = currentTest.Questions.ElementAt(i - 1).Id,
            //        SelectedAnswers = "",
            //    });

            //_testSessions.Add(currentUser.TestSessions.Last());
            currentTestSession = context.TestSessions.OrderBy(ts => ts.Id).Last();

            return currentTestSession.Id;
        }

        public void SubmitTestSession(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public AnswerDetailVM UpdateAnswer(int questionId, int answerId, string answerText, int sortOrder, bool isCorrect)
        {
            var answer = context.Answers.SingleOrDefault(o => o.Id == answerId);
            var questionType = context.Questions.SingleOrDefault(o => o.Id == questionId).QuestionType;

            answer.AnswerText = answerText;
            answer.IsCorrect = isCorrect;
            answer.SortOrder = sortOrder;

            var model = new AnswerDetailVM()
            {
                AnswerId = answerId,
                AnswerText = answerText,
                IsChecked = isCorrect,
                QuestionType = questionType,
                SortOrder = answer.SortOrder
            };
            context.SaveChanges();

            return model;
        }

        public void UpdateQuestion(int testId, int questionId, EditQuestionVM viewModel)
        {
            var question = context.Questions.SingleOrDefault(o => o.Id == questionId);
            question.SortOrder = viewModel.SortOrder;
            question.QuestionType = viewModel.Type;
            question.HasComment = viewModel.HasComment;
            question.QuestionText = viewModel.QuestionText;
            context.SaveChanges();
        }

        public bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment)
        {
            throw new NotImplementedException();
        }

        private User GetUser(string userName)
        {
            var currentUser = identityContext.Users.Single(o => o.UserName == userName);
            return context.Users
                //.Include(u=>u.TestSessions)
                //.ThenInclude(ts=>ts.QuestionResults)
                .Single(o => o.UserId == currentUser.Id);
        }
    }
}
