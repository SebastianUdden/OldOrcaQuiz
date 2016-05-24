using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models;
using OrcaQuiz.Models.Enums;
using OrcaQuiz.Utils;
using OrcaQuiz.ViewModels;

namespace OrcaQuiz.Repositories
{
    public class TestRepository : IOrcaQuizRepository
    {
        public List<Module> _modules { get; set; }
        public List<Test> _tests { get; set; }
        public List<User> _users { get; set; }
        public List<Answer> _answers { get; set; }
        public List<TestSession> _testSessions { get; set; }
        public List<QuestionResult> _questionResults { get; set; }


        public List<Module> GetAllModules ()
        {
            return _modules;
        }

        public Module GetModuleById(int Id)
        {
            return _modules.Single(o => o.Id == Id);
        }

        //public Test[] GetAllTests()
        //{
        //    return (_modules.SelectMany(o => o.Tests)).ToArray();
        //}

        public Question[] GetAllQuestions()
        {
            return (_tests.SelectMany(o => o.Questions)).ToArray();
        }

        public Answer[] GetAllAnswers()
        {
            return (_tests.SelectMany(o => o.Questions).SelectMany(q => q.Answers)).ToArray();
        }

        public TestRepository()
        {
            _modules = new List<Module>();
            _tests = new List<Test>();
            _users = new List<User>();
            _answers = new List<Answer>();
            _testSessions = new List<TestSession>();
            _questionResults = new List<QuestionResult>();

            #region Add static users
            _users.Add(new User()
            {
                Id = 1,
                Email = "linus.joensson.ms@outlook.com",
                FirstName = "Linus",
                Lastname = "Joensson",
            });
            _users.Add(new User()
            {
                Id = 2,
                Email = "sebastian.udden@gmail.com",
                FirstName = "Sebastian",
                Lastname = "Uddén",
            });
            _users.Add(new User()
            {
                Id = 3,
                Email = "mattiashagelin@outlook.com",
                FirstName = "Mattias",
                Lastname = "Hagelin",
            });
            _users.Add(new User()
            {
                Id = 4,
                Email = "patrikweibus@outlook.com",
                FirstName = "Patrik",
                Lastname = "Weibus",
            });
            //_users.Last().TestSessions.Add(_testSessions.Last());
            #endregion

            #region Add static modules
            _modules.Add(new Module()
            {
                Id = 1,
                Name = "Programming",
                Description = "A collection of programmer tests.",
                Tags = "C#, SQL, Java"
            });
            _modules.Add(new Module()
            {
                Id = 2,
                Name = "Football",
                Description = "A series of Zlatan tests",
                Tags = "Zlatan, Ibra, Fussball"
            });
            #endregion

            #region Add static tests
            #region Test 1
            _tests.Add(new Test()
            {
                Id = 1,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                AuthorId = "Linus Joensson",
                Name = "Basic C#",
                Description = "An eazy test",
                CertificateAuthor = "Patrik J",
                CertificateCompany = "Pattzor",
                CertificateCustomText = "GRTZ",
                #region Questions
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        TestId = 1,
                        Id = 1,
                        QuestionText = /*@"<iframe src=""//www.youtube.com/embed/ncclpqQzjY0"" width=""auto"" height=""auto"" allowfullscreen=""allowfullscreen""></iframe>"*/"How do you write into console.",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "C#" + "," + "hard",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = 1, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "Console.WriteLine()" },
                        new Answer() { Id = 2, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "Console.ReadLine()" }
                        }
                    },
                    new Question()
                    {
                        TestId = 1,
                        Id = 2,
                        QuestionText = "What is the meaning of life?",
                        QuestionType = QuestionType.SingleChoice,
                        Tags = "Life" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = 3, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "I don't know, death?" },
                        new Answer() { Id = 4, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "42" }
                        }
                    },
                    new Question()
                    {
                        TestId = 1,
                        Id = 3,
                        QuestionText = "Who can survive an atomic blast?",
                        QuestionType = QuestionType.MultipleChoice,
                        Tags = "Life" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                            new Answer() { Id = 5, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "Arnold Schwarzenegger" },
                            new Answer() { Id = 6, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "A cockroach"
                            },
                            new Answer() { Id = 7, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "Chuck Norris"
                            },
                            new Answer() { Id = 8, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "Amy Diamond"
                            }
                        }
                    }
                },
                #endregion
                TimeLimitInMinutes = 3,
                PassPercentage = 70,
                TestSessions = new List<TestSession>()
            });
            #endregion
            #region Test 2
            _tests.Add(new Test()
            {
                Id = 2,
                //Tags = new List<string>() { "Eazy", "awesome", "heavy" },
                AuthorId = "Linus Joensson",
                Name = "My Second Test",
                Description = "An eazy test",
                #region Questions
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        TestId = 2,
                        Id = 4,
                        QuestionText = "What is a variable?",
                        QuestionType = QuestionType.MultipleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                        new Answer() { Id = 9, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = true, AnswerText = "A store of value" },
                        new Answer() { Id = 10, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "A banana " },
                        new Answer() { Id = 11, QuestionId = GetAllQuestions().Count() + 1,  IsCorrect = false, AnswerText = "All of the above " },
                        }
                    },
                    new Question()
                    {
                        TestId = 2,
                        Id = 5,
                        QuestionText = @"<iframe src=""//www.youtube.com/embed/ncclpqQzjY0"" width=""560"" height=""314"" allowfullscreen=""allowfullscreen""></iframe>",
                        QuestionType = QuestionType.MultipleChoice,
                        Tags = "Music" + "," + "medium",
                        Author = "Sebastian Uddén",
                        Answers = new List<Answer>()
                        {
                            new Answer() { Id = 12, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = true, AnswerText = "Zlatan" },
                            new Answer() { Id = 13, QuestionId = GetAllQuestions().Count() + 2,  IsCorrect = true, AnswerText = "Zlatan" }
                        }
                    },

                },
                #endregion
                TimeLimitInMinutes = 10,
                PassPercentage = 50,
                TestSessions = new List<TestSession>()
            });
            #endregion
            #endregion

            #region Add Testsessions
            #region Testsession 1
            _tests[0].TestSessions.Add(
                    new TestSession
                    {
                        Id = 1,
                        TestId = 1,
                        UserId = 1,
                        QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 1,
                                Comment = null,
                                QuestionId = 1,
                                SelectedAnswers = "2"
                            },
                            new QuestionResult
                            {
                                Id = 2,
                                Comment = null,
                                QuestionId = 2,
                                SelectedAnswers = "3"
                            },
                            new QuestionResult
                            {
                                Id = 3,
                                Comment = null,
                                QuestionId = 3,
                                SelectedAnswers = "8"
                            }
                        },
                        SecondsLeft = 0,
                        StartTime = DateTime.Now,
                        SubmitTime = DateTime.Now.AddMinutes(10),
                        User = _users.Single(o => o.Id == 1)
                    });

            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 2,
                    TestId = 1,
                    UserId = 2,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 4,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "1"
                            },
                            new QuestionResult
                            {
                                Id = 5,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "4"
                            },
                             new QuestionResult
                            {
                                Id = 6,
                                Comment = null,
                                QuestionId = 3,
                                SelectedAnswers = "5,6,7"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(8),
                    User = _users.Single(o => o.Id == 2)
                });
            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 3,
                    TestId = 1,
                    UserId = 3,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 7,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "1"
                            },
                            new QuestionResult
                            {
                                Id = 8,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "4"
                            },
                            new QuestionResult
                            {
                                Id = 9,
                                Comment = null,
                                QuestionId = 3,
                                SelectedAnswers = "5,6,8"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(1),
                    User = _users.Single(o => o.Id == 3)
                });
            _tests[0].TestSessions.Add(
                new TestSession
                {
                    Id = 4,
                    TestId = 1,
                    UserId = 4,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 10,
                                Comment = null,
                                QuestionId = 1,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 1),
                                SelectedAnswers = "2"
                            },
                            new QuestionResult
                            {
                                Id = 11,
                                Comment = null,
                                QuestionId = 2,
                                //Question = _tests.Single(o=>o.Id == 1).Questions.Single(o=>o.Id == 2),
                                SelectedAnswers = "4"
                            },
                            new QuestionResult
                            {
                                Id = 12,
                                Comment = null,
                                QuestionId = 3,
                                SelectedAnswers = "5,6"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(30),
                    User = _users.Single(o => o.Id == 4)
                });
            #endregion
            #region Testsession 2
            _tests[1].TestSessions.Add(
                    new TestSession
                    {
                        Id = 1,
                        TestId = 2,
                        UserId = 1,
                        QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 13,
                                Comment = null,
                                QuestionId = 4,
                                SelectedAnswers = "10"
                            },
                            new QuestionResult
                            {
                                Id = 14,
                                Comment = null,
                                QuestionId = 5,
                                SelectedAnswers = "13"
                            }
                        },
                        StartTime = DateTime.Now,
                        SubmitTime = DateTime.Now.AddMinutes(10),
                        User = _users.Single(o => o.Id == 1)
                    });

            _tests[1].TestSessions.Add(
                new TestSession
                {
                    Id = 2,
                    TestId = 2,
                    UserId = 2,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 15,
                                Comment = null,
                                QuestionId = 4,
                                SelectedAnswers = "9"
                            },
                            new QuestionResult
                            {
                                Id = 16,
                                Comment = null,
                                QuestionId = 5,
                                SelectedAnswers = "12,13"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(8),
                    User = _users.Single(o => o.Id == 2)
                });
            _tests[1].TestSessions.Add(
                new TestSession
                {
                    Id = 3,
                    TestId = 2,
                    UserId = 3,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 17,
                                Comment = null,
                                QuestionId = 4,
                                SelectedAnswers = "9"
                            },
                            new QuestionResult
                            {
                                Id = 18,
                                Comment = null,
                                QuestionId = 5,
                                SelectedAnswers = "13"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(1),
                    User = _users.Single(o => o.Id == 3)
                });
            _tests[1].TestSessions.Add(
                new TestSession
                {
                    Id = 4,
                    TestId = 2,
                    UserId = 4,
                    QuestionResults = new List<QuestionResult>
                        {
                            new QuestionResult
                            {
                                Id = 19,
                                Comment = null,
                                QuestionId = 4,
                                SelectedAnswers = "11"
                            },
                            new QuestionResult
                            {
                                Id = 20,
                                Comment = null,
                                QuestionId = 5,
                                SelectedAnswers = "12,13"
                            }
                        },
                    StartTime = DateTime.Now,
                    SubmitTime = DateTime.Now.AddMinutes(30),
                    User = _users.Single(o => o.Id == 4)
                });
            #endregion
            #endregion

            _modules.Single(o => o.Id == 1).Tests.Add(_tests.Single(o => o.Id == 1));
            _modules.Single(o => o.Id == 1).Tests.Add(_tests.Single(o => o.Id == 2));
            //_modules.Single(o => o.Id == 1).Tests.Add(_tests[0]);
            //_modules[0].Tests.Add(_tests[1]);
        }

        public int CreateTest(Test test)
        {
            _tests.Add(new Test()
            {
                //Dynamic
                Id = _tests.Count + 1,
                Description = test.Description,
                Questions = new List<Question>(),
                Name = test.Name,
                Tags = test.Tags,
                TimeLimitInMinutes = test.TimeLimitInMinutes,
                ShowPassOrFail = test.ShowPassOrFail,
                ShowTestScore = test.ShowTestScore,
                PassPercentage = test.PassPercentage,
                NumberOfFeaturedQuestions = test.NumberOfFeaturedQuestions,
                CertificateAuthor = test.CertificateAuthor,
                CertificateCompany = test.CertificateCompany,
                CertificateCustomText = test.CertificateCustomText,
                CertTemplatePath = test.CertTemplatePath,
                CustomCompletionMessage = test.CustomCompletionMessage,
                EnableCertDownloadOnCompletion = test.EnableCertDownloadOnCompletion,
                EnableEmailCertOnCompletion = test.EnableEmailCertOnCompletion,

                //Static
                IsPublished = true,
                //Tags = new List<string>() { "happy", "insane" },
                AuthorId = _users.ElementAt(0).FirstName,
            });

            return _tests.Last().Id;
        }

        public void CopyTestToModule(int testId, int moduleId)
        {
            var thisModule = _modules.Single(o => o.Id == moduleId);
            var thisTest = GetAllTests().Single(o => o.Id == testId);

            thisModule.Tests.Add(new Test
            {
                    Name = thisTest.Name,
                    Description = thisTest.Description,
                    Questions = thisTest.Questions,
                    Id = GetAllTests().Count() + 1,
                    ModuleId = moduleId
            });
        

            //needsediting
        }

        public void RemoveTestFromModule(int testId, int moduleId)
        {
            var thisModule = _modules.Single(o => o.Id == moduleId);
            thisModule.Tests.RemoveAll(o => o.Id == testId);

        }

        public void CopyQuestionToTest(int questionId, int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);

            thisTest.Questions.Add(new Question()
            {
                //Duplicate original question
                Answers = thisQuestion.Answers,
                QuestionType = thisQuestion.QuestionType,
                Tags = thisQuestion.Tags,
                QuestionText = thisQuestion.QuestionText,
                HasComment = thisQuestion.HasComment,

                //New question id
                Id = GetAllQuestions().Count() + 1,

                //Add specific properties
                SortOrder = thisQuestion.SortOrder,
                CreatedDate = DateTime.UtcNow,
                Author = _users.ElementAt(0).FirstName,

                //Question belongs to this test
                TestId = testId

            });

        }

        public Test[] GetAllTests()
        {
            return _tests.ToArray();
        }

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.OrderBy(o => o.SortOrder).ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.SingleOrDefault(o => o.QuestionId == thisQuestion.Id);

            //var timeLeft = thisTest.TimeLimit - (DateTime.UtcNow - thisTestSession.StartTime);

            //var secondsLeft = TimeUtils.GetSecondsLeft(thisTest.TimeLimitInMinutes, thisTestSession.StartTime);
            var selectedAnswers = thisQuestionResult?.SelectedAnswers.Split(',');

            return new ViewQuestionVM()
            {
                TestId = thisTest.Id,
                TestTitle = thisTest.Name,
                NumOfQuestion = thisTest.Questions.Count(),
                QuestionIndex = questionIndex,
                SecondsLeft = thisTestSession.SecondsLeft,

                QuestionFormVM = new QuestionFormVM()
                {
                    IsInTestSession = isInSession,
                    QuestionType = thisQuestion.QuestionType,
                    QuestionText = thisQuestion.QuestionText,
                    HasComment = thisQuestion.HasComment,
                    Comment = thisQuestionResult?.Comment,
                    SelectedAnswers = selectedAnswers,
                    Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
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

        public SessionIndexVM GetSessionIndexVM(int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            var thisUserId = 1;

            var viewModel = new SessionIndexVM()
            {
                UserId = thisUserId,
                TestId = thisTest.Id,
                NumberOfQuestions = thisTest.Questions.Count(),
                TestDescription = thisTest.Description,
                TestName = thisTest.Name,
                TimeLimit = thisTest.TimeLimitInMinutes
            };

            return viewModel;
        }

        public int StartNewSession(int userId, int testId)
        {
            var thisUser = _users.Single(o => o.Id == userId);
            var thisTest = _tests.Single(o => o.Id == testId);

            thisUser.TestSessions.Add(new TestSession()
            {
                Id = _testSessions.Count() + 1,
                QuestionResults = new List<QuestionResult>(),
                StartTime = DateTime.UtcNow,
                SecondsLeft = thisTest.TimeLimitInMinutes * 60,
                TestId = testId,
                UserId = userId,
            });

            for (int i = 1; i <= thisTest.Questions.Count(); i++)
                thisUser.TestSessions.Last().QuestionResults.Add(new QuestionResult()
                {
                    Id = _questionResults.Count() + i,
                    QuestionId = thisTest.Questions.ElementAt(i - 1).Id,
                    SelectedAnswers = "",
                });

            _testSessions.Add(thisUser.TestSessions.Last());

            return _testSessions.Last().Id;
        }

        public bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment)
        {
            var thisTestSession = _testSessions.Single(o => o.Id == testSessionId);
            var thisTest = _tests.Single(o => o.Id == thisTestSession.TestId);
            var thisQuestion = thisTest.Questions.ElementAt(questionIndex - 1);
            var thisQuestionResult = thisTestSession.QuestionResults.Find(o => o.QuestionId == thisQuestion.Id);

            var hasTimeLeft = TimeUtils.HasTimeLeft(thisTest.TimeLimitInMinutes, thisTestSession.StartTime);

            if (hasTimeLeft)
            {
                if (thisQuestionResult == null)
                {
                    thisTestSession.QuestionResults.Add(new QuestionResult()
                    {
                        Id = _questionResults.Count() + 1,
                        QuestionId = thisQuestion.Id,
                    });
                    _questionResults.Add(thisTestSession.QuestionResults.Last());
                    thisQuestionResult = thisTestSession.QuestionResults.Last();
                }

                thisQuestionResult.SelectedAnswers = "";

                if (selectedAnswers != null)
                {
                    foreach (var answer in selectedAnswers)
                        thisQuestionResult.SelectedAnswers += answer + ",";
                    thisQuestionResult.SelectedAnswers = thisQuestionResult.SelectedAnswers.Substring(0, thisQuestionResult.SelectedAnswers.Length - 1);
                }
                if (!string.IsNullOrWhiteSpace(comment))
                    thisQuestionResult.Comment = comment;
            }

            return (hasTimeLeft);
        }

        public void SubmitTestSession(int testSessionId)
        {
            var thisSession = _testSessions.Single(o => o.Id == testSessionId);
            thisSession.SubmitTime = DateTime.UtcNow;
        }

        public TestSession GetTestSessionById(int testSessionId)
        {
            return _testSessions.Single(o => o.Id == testSessionId);
        }

        public void RemoveQuestionFromTest(int questionId, int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);
            thisTest.Questions.RemoveAll(o => o.Id == questionId);

        }

        public ManageTestQuestionsVM GetManageTestQuestionVM(int testId)
        {
            var thisTest = _tests.Single(o => o.Id == testId);

            var viewModel = new ManageTestQuestionsVM()
            {
                TestId = testId,
                Description = thisTest.Description,
                Questions = thisTest.Questions.OrderBy(o => o.SortOrder).ToList(),
                TestName = thisTest.Name,
            };

            return viewModel;
        }

        public QuestionFormVM GetPreviewQuestion(int questionId)
        {
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);
            var viewModel = new QuestionFormVM()
            {
                QuestionText = thisQuestion.QuestionText
            };

            return viewModel;
        }

        public int CreateTestQuestion(int testId)
        {
            var newQuestion = new Question()
            {
                TestId = testId,
                Id = GetAllQuestions().Count() + 1,
                CreatedDate = DateTime.UtcNow,
            };

            _tests.SingleOrDefault(o => o.Id == testId).Questions.Add(newQuestion);

            return newQuestion.Id;
        }

        public int CreateAnswer(int questionId, AnswerDetailVM viewModel)
        {
            var answer = new Answer()
            {
                Id = GetAllAnswers().Count() + 1,
                AnswerText = viewModel.AnswerText,
                IsCorrect = viewModel.ShowAsCorrect,
                QuestionId = questionId,
            };

            GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
                .Answers.Add(answer);

            return answer.Id;
        }

        public int CreateAnswer(int questionId)
        {
            var answer = new Answer()
            {
                AnswerText = "New answer...",
                Id = GetAllAnswers().Count() + 1,
                SortOrder = _answers.Count() > 0 ? _answers.Max(o => o.SortOrder) + 10 : 10
            };

            _answers.Add(answer);
            GetAllQuestions().SingleOrDefault(o => o.Id == questionId)?
                .Answers.Add(answer);

            return answer.Id;
        }

        public void RemoveAnswerFromQuestion(int testId, int questionId, int answerId)
        {
            var thisQuestion = _tests.Single(o => o.Id == testId).Questions.Single(q => q.Id == questionId);
            thisQuestion.Answers.RemoveAll(a => a.Id == answerId);

            _answers.RemoveAll(a => a.Id == answerId);
        }

        public SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason)
        {
            var user = _users.Single(u => _testSessions.Single(o => o.Id == testSessionId).UserId == u.Id);
            var testSession = _testSessions.Single(o => o.Id == testSessionId);
            var test = _tests.Single(o => o.Id == testSession.TestId);
            var questionsAndAnswers = new QuestionsAndAnswersUtils();
            questionsAndAnswers.Questions = GetAllQuestions();
            questionsAndAnswers.Answers = GetAllAnswers();
            return new SessionCompletedVM()
            {
                TestSessionId = testSessionId,
                Date = DateTime.Now.Date.ToString("dd/MM/yyyy"),
                IsSuccessful = GradeUtils.CheckHasPassed(testSession, test.PassPercentage , questionsAndAnswers),
                UserName = $"{user.FirstName} {user.Lastname}",
                SessionCompletedReason = sessionCompletedReason
            };
        }

        public EditQuestionVM GetEditQuestionVM(int testId, int questionId)
        {
            var thisQuestion = GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
            var testQuestions = GetAllQuestions().Where(o => o.TestId == testId);

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

        public QuestionFormVM GetPreviewQuestionPartial(int questionId)
        {
            var thisQuestion = GetAllQuestions().Single(o => o.Id == questionId);

            var viewModel = new QuestionFormVM()
            {
                IsInTestSession = false,
                Answers = thisQuestion.Answers.Select(o => new AnswerDetailVM()
                {
                    AnswerId = o.Id,
                    AnswerText = o.AnswerText,
                    ShowAsCorrect = o.IsCorrect,
                    IsChecked = o.IsCorrect
                }).ToList(),
                QuestionText = thisQuestion.QuestionText,
                HasComment = thisQuestion.HasComment,
                QuestionType = thisQuestion.QuestionType
            };

            return viewModel;
        }

        public ShowResultsVM GetShowResultsVM(int testId)
        {
            var test = _tests.Single(o => o.Id == testId);

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
                    testscore = TestSessionUtils.GetScore(ts, GetAllAnswers(), GetAllQuestions())
                }).ToArray()
            };
            return new ShowResultsVM
            {
                ResultDataJSON = JsonConvert.SerializeObject(result)
            };
        }

        public PdfSymbols GetCertificateSymbols(int testSessionId)
        {
            var session = _testSessions.Single(o => o.Id == testSessionId);
            var test = _tests.Single(o => o.Id == session.TestId);
            var user = _users.Single(o => o.Id == session.UserId);
            return new PdfSymbols
            {
                Author = test.CertificateAuthor,
                Company = test.CertificateCompany,
                CertificateName = test.Name,
                Date = session.StartTime.ToString("yyyy-MM-dd"),
                Details = test.CertificateCustomText,
                StudentName = user.FirstName + " " + user.Lastname
            };
        }

        public ManageModuleTestsVM GetManageModuleTestVM(int moduleId)
        {
            var module = _modules.Single(o => o.Id == moduleId);

            return new ManageModuleTestsVM
            {
                ModuleId = module.Id,
                Description = module.Description,
                ModuleName = module.Name,
                Tests = module.Tests
            };
        }
    }
}
