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
        public void CopyQuestionToTest(int questionId, int testId, string username)
        {
            //var test = context.Tests.Single(o => o.Id == testId);
            var question = context.Questions
                .Include(o => o.Answers)
                .Single(o => o.Id == questionId);

            //var user = context.Users.Single(o => o.UserId == identityContext.Users
            //    .Single(iu => iu.UserName == userName).Id);

            var currentUser = identityContext.Users.Single(o => o.UserName == username);
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
        public DashboardVM GetDashboardVM()
        {
            return new DashboardVM
            {
                Tests = context.Tests.Select(o => new DashboardTestItemVM
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description
                }).ToList()
            };
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

        //public int CreateTest(Test test)
        //{
        //    test.AuthorId = context.Users.Single(o => o.Email == test.Tags).Id;
        //    context.Add(test);
        //    context.SaveChanges();

        //    return context.Tests.Last().Id;
        //}

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

        public void CreateNewModule(ModuleVM model)
        {
            context.Modules.Add(new Module()
            {
                Description = model.Description,
                Name = model.Name,
                Tags = model.Tags
                //,Tests = model.Tests
            });
            context.SaveChanges();
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
            var testSession = context.TestSessions
                .Include(ts => ts.Test)
                .Include(ts => ts.User)
                .SingleOrDefault(ts => ts.Id == testSessionId);
            
            return new PdfSymbols
            {
                Author = testSession.Test.CertificateAuthor,
                Company = testSession.Test.CertificateCompany,
                CertificateName = testSession.Test.Name,
                Date = testSession.StartTime.ToString("yyyy-MM-dd"),
                Details = testSession.Test.CertificateCustomText,
                StudentName = $"{testSession.User.FirstName} {testSession.User.Lastname}"
            };
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
            var thisQuestion = context.Questions
                .Include(q=>q.Answers)
                .SingleOrDefault(o => o.Id == questionId);
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
            return context.Modules
                .Include(m => m.Tests)
                .Where(m=>m.Id == moduleId)
                .Select(m => new ManageModuleTestsVM()
                {
                    ModuleId = m.Id,
                    Description = m.Description,
                    ModuleName = m.Name,
                    Tests = m.Tests
                })
                .Single();
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

        public ModuleVM GetModuleVMByModuleId(int moduleId)
        {
            return context.Modules
                .Include(m=>m.Tests)
                .Where(m=>m.Id == moduleId)
                .Select(m=> new ModuleVM()
                {
                    Id = m.Id,
                    Description = m.Description,
                    Name = m.Name,
                    Tags = m.Tags,
                    Tests = m.Tests
                })
                .Single();

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
            var testSession = context.TestSessions
                .Include(ts => ts.Test)
                    .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                .Include(ts => ts.User)
                .Single(ts => ts.Id == testSessionId);


            //var user = _users.Single(u => _testSessions.Single(o => o.Id == testSessionId).UserId == u.Id);
            //var testSession = _testSessions.Single(o => o.Id == testSessionId);
            //var test = _tests.Single(o => o.Id == testSession.TestId);
            var questionsAndAnswers = new QuestionsAndAnswersUtils()
            {
                Questions = context.Questions.ToArray(),
                Answers = context.Answers.ToArray()
            };
            return new SessionCompletedVM()
            {
                TestSessionId = testSessionId,
                Date = DateTime.Now.Date.ToString("dd/MM/yyyy"),
                IsSuccessful = GradeUtils.CheckHasPassed(testSession, testSession.Test.PassPercentage, questionsAndAnswers),
                UserName = $"{testSession.User.FirstName} {testSession.User.Lastname}",
                SessionCompletedReason = sessionCompletedReason
            };
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
                .Include(t => t.Questions)
                .Include(t => t.TestSessions)
                    .ThenInclude(ts => ts.QuestionResults)
                .Include(t => t.TestSessions)
                    .ThenInclude(ts => ts.User)
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
            return context.TestSessions.Single(ts => ts.Id == testSessionId);
        }

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            var currentTestSession = context.TestSessions
                .Include(ts => ts.QuestionResults)
                .Single(o => o.Id == testSessionId);
            var currentTest = context.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
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
            var currentTestSession = new TestSession()
            {
                StartTime = DateTime.UtcNow,
                TestId = currentTest.Id,
                UserId = currentUser.Id
            };

            context.TestSessions.Add(currentTestSession);
            var result = await context.SaveChangesAsync();

            for (int i = 0; i < currentTest.Questions.Count(); i++)
            {
                context.QuestionResults.Add(new QuestionResult
                {
                    QuestionId = currentTest.Questions[i].Id,
                    SelectedAnswers = "",
                    TestSessionId = currentTestSession.Id
                });
            }
            await context.SaveChangesAsync();

            return currentTestSession.Id;
        }

        public void SubmitTestSession(int testSessionId)
        {
            var currentSession = context.TestSessions.Single(o => o.Id == testSessionId);
            currentSession.SubmitTime = DateTime.UtcNow;
            context.SaveChanges();
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

        public void UpdateModule(ModuleVM model)
        {
            var module = context.Modules.Single(m => m.Id == model.Id);
            module.Name = model.Name;
            module.Description = model.Description;
            module.Tags = model.Tags;
            context.SaveChanges();
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
            var currentTestSession = context.TestSessions
                .Include(ts => ts.Test)
                    .ThenInclude(t => t.Questions)
                .Include(ts => ts.QuestionResults)
                .Single(ts => ts.Id == testSessionId);

            var currentQuestion = currentTestSession.Test
                .Questions.OrderBy(q => q.SortOrder).ElementAt(questionIndex - 1);

            var currentQuestionResult = context.QuestionResults.Single(qr => qr.QuestionId == currentQuestion.Id && qr.TestSessionId == testSessionId);

            //var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            //var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            //var thisQuestion = thisTest.Questions.ElementAt(questionIndex - 1);
            //var thisQuestionResult = thisTestSession.QuestionResults.Find(o => o.QuestionId == thisQuestion.Id);

            var hasTimeLeft = TimeUtils.HasTimeLeft(currentTestSession.Test.TimeLimitInMinutes, currentTestSession.StartTime);

            if (hasTimeLeft)
            {
                if (currentQuestionResult == null)
                {
                    var newQuestionResult = new QuestionResult()
                    {
                        QuestionId = currentQuestion.Id,
                        TestSessionId = currentTestSession.Id,
                    };
                    context.QuestionResults.Add(newQuestionResult);
                    context.SaveChanges();

                    //currentTestSession.QuestionResults.Add(new QuestionResult()
                    //{
                    //    QuestionId = thisQuestion.Id,
                    //});
                    //_questionResults.Add(thisTestSession.QuestionResults.Last());
                    //thisQuestionResult = thisTestSession.QuestionResults.Last();
                    currentQuestionResult = newQuestionResult;
                }

                currentQuestionResult.SelectedAnswers = "";

                if (selectedAnswers != null)
                {
                    for (int i = 0; i < selectedAnswers.Length; i++)
                    {
                        if (i == selectedAnswers.Length - 1)
                            currentQuestionResult.SelectedAnswers += selectedAnswers[i];
                        else
                            currentQuestionResult.SelectedAnswers += selectedAnswers[i] + ",";
                    }
                    //foreach (var answer in selectedAnswers)
                    //    currentQuestionResult.SelectedAnswers += answer + ",";
                    //currentQuestionResult.SelectedAnswers = thisQuestionResult.SelectedAnswers.Substring(0, thisQuestionResult.SelectedAnswers.Length - 1);
                }
                if (!string.IsNullOrWhiteSpace(comment))
                    currentQuestionResult.Comment = comment;
            }
            context.SaveChanges();

            return (hasTimeLeft);
        }

        private User GetUser(string userName)
        {
            var currentUser = identityContext.Users.Single(o => o.UserName == userName);
            return context.Users
                //.Include(u=>u.TestSessions)
                //.ThenInclude(ts=>ts.QuestionResults)
                .Single(o => o.UserId == currentUser.Id);
        }

        public ModuleVM[] GetAllModules()
        {
            return context.Modules
                .Select(o => new ModuleVM
                {
                    Name = o.Name,
                    Description = o.Description,
                    Tags = o.Tags,
                    Tests = o.Tests,
                    Id = o.Id
                })
                .ToArray();
        }

        public TestSettingsFormVM GetTestSettingsFormVM(int testId)
        {
            return context.Tests.Where(o => o.Id == testId)
                .Select(model => new TestSettingsFormVM
                {
                    TestName = model.Name,
                    Description = model.Description,
                    Tags = model.Tags,
                    ShowPassOrFail = model.ShowPassOrFail,
                    ShowTestScore = model.ShowTestScore,
                    CustomCompletionMessage = model.CustomCompletionMessage,
                    TimeLimitInMinutes = model.TimeLimitInMinutes,
                    PassPercentage = model.PassPercentage,
                    NumberOfFeaturedQuestions = model.NumberOfFeaturedQuestions,
                    CertificateAuthor = model.CertificateAuthor,
                    CertificateCompany = model.CertificateCompany,
                    CertificateCustomText = model.CertificateCustomText,
                    CertificateTemplatePath = model.CertificateTemplatePath,
                    EnableCertificateDownloadOnCompletion = model.EnableCertificateDownloadOnCompletion,
                    EnableEmailCertificateOnCompletion = model.EnableEmailCertificateOnCompletion
                })
                .SingleOrDefault();
        }

        public void UpdateTestSettings(TestSettingsFormVM viewModel, int testId)
        {
            var thisTest = context.Tests.SingleOrDefault(o => o.Id == testId);
            thisTest.Description = viewModel.Description;
            thisTest.Name = viewModel.TestName;
            thisTest.Tags = viewModel.Tags;
            thisTest.ShowPassOrFail = viewModel.ShowPassOrFail;
            thisTest.ShowTestScore = viewModel.ShowTestScore;
            thisTest.CustomCompletionMessage = viewModel.CustomCompletionMessage;
            thisTest.TimeLimitInMinutes = viewModel.TimeLimitInMinutes;
            thisTest.PassPercentage = viewModel.PassPercentage;
            thisTest.NumberOfFeaturedQuestions = viewModel.NumberOfFeaturedQuestions;
            thisTest.CertificateAuthor = viewModel.CertificateAuthor;
            thisTest.CertificateCompany = viewModel.CertificateCompany;
            thisTest.CertificateCustomText = viewModel.CertificateCustomText;
            thisTest.CertificateTemplatePath = viewModel.CertificateTemplatePath;
            thisTest.EnableCertificateDownloadOnCompletion = viewModel.EnableCertificateDownloadOnCompletion;
            thisTest.EnableEmailCertificateOnCompletion = viewModel.EnableEmailCertificateOnCompletion;
        }

        public int CreateTest(TestSettingsFormVM model, string username)
        {
            var authorId = GetUserIdByUsername(username);
            var test = new Test()
            {
                AuthorId = authorId,
                Name = model.TestName,
                Description = model.Description,
                Tags = null,
                ShowPassOrFail = model.ShowPassOrFail,
                ShowTestScore = model.ShowTestScore,
                CustomCompletionMessage = model.CustomCompletionMessage,
                TimeLimitInMinutes = model.TimeLimitInMinutes,
                PassPercentage = model.PassPercentage,
                NumberOfFeaturedQuestions = model.NumberOfFeaturedQuestions,
                CertificateAuthor = model.CertificateAuthor,
                CertificateCompany = model.CertificateCompany,
                CertificateCustomText = model.CertificateCustomText,
                CertificateTemplatePath = model.CertificateTemplatePath,
                EnableCertificateDownloadOnCompletion = model.EnableCertificateDownloadOnCompletion,
                EnableEmailCertificateOnCompletion = model.EnableEmailCertificateOnCompletion
            };
            context.Tests.Add(test);
            context.SaveChanges();
            return test.Id;
        }

        public int GetUserIdByUsername(string username)
        {
            var aspNetUserId = identityContext.Users.Single(o => o.UserName == username).Id;
            var userId = context.Users.Single(o => o.UserId == aspNetUserId).Id;

            return userId;
        }
    }
}
