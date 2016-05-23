using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.Repositories;
using TestPlatform.Utils;
using TestPlatform.ViewModels;

namespace TestPlatform.Controllers
{
    public class AdminController : Controller
    {
        ITestPlatformRepository repository;
        IHostingEnvironment env;

        public AdminController(ITestPlatformRepository repository, IHostingEnvironment env)
        {
            this.env = env;
            this.repository = repository;
        }

        public IActionResult Dashboard()
        {
            //PdfUtils.GenerateCerfificate(env, "OrcaQuizTemplate.pdf", "cerBOficat2.pdf", new PdfSymbols { CertificatName = "C#.NET", Author = "Pontus Wittemark", Company = "WarmKitten", Details = "Bacon ipsum dolor amet filet mignon brisket Bacon ipsum dolor amet filet mignon brisket Bacon ipsum dolor amet filet mignon brisket", StudentName = "Mikael Brunnberg" });

            var model = repository.GetAllTests();
            var viewModel = new DashboardVM()
            {
                Tests = model.ToList()
            };
            return View(viewModel);
        }

        [Route("Admin/ShowResults/{testId}")]
        public IActionResult ShowResults(int testId)
        {
            var vm = repository.GetShowResultsVM(testId);
            return View(vm);
        }

        [Route("Admin/Test/{testId}/Question/Create")]
        public IActionResult CreateQuestion(int testId)
        {
            int questionId = repository.CreateTestQuestion(testId);

            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        [HttpPost]
        public IActionResult UpdateQuestionSettings(int testId, int questionId, EditQuestionVM viewModel)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
            thisQuestion.SortOrder = viewModel.SortOrder;
            thisQuestion.QuestionType = viewModel.Type;
            thisQuestion.HasComment = viewModel.HasComment;
            thisQuestion.QuestionText = viewModel.QuestionText;

            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        //[HttpPost]
        //public PartialViewResult UpdateQuestionText(int questionId, string questionText)
        //{
        //    var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);
        //    thisQuestion.QuestionText = questionText;

        //    var model = new QuestionFormVM()
        //    {
        //        QuestionText = questionText,
        //        IsInEditQuestion = true,
        //        QuestionType = thisQuestion.QuestionType
        //    };

        //    return PartialView("_QuestionFormPartial", model);
        //}

        public PartialViewResult UpdateAnswer(int questionId, int answerId, string answerText, int sortOrder, bool isCorrect)
        {
            var thisAnswer = repository.GetAllAnswers().SingleOrDefault(o => o.Id == answerId);
            var thisQuestionType = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId).QuestionType;

            thisAnswer.AnswerText = answerText;
            thisAnswer.IsCorrect = isCorrect;
            thisAnswer.SortOrder = sortOrder;

            var model = new AnswerDetailVM()
            {
                AnswerId = answerId,
                AnswerText = answerText,
                IsChecked = isCorrect,
                QuestionType = thisQuestionType,
                SortOrder = thisAnswer.SortOrder
            };

            return PartialView("_AnswerFormPartial", model);
        }

        [HttpPost]
        public PartialViewResult EditQuestionText(int questionId)
        {
            var thisQuestion = repository.GetAllQuestions().SingleOrDefault(o => o.Id == questionId);

            var model = new QuestionFormVM()
            {
                QuestionText = thisQuestion.QuestionText,
                IsInEditQuestion = true,
                QuestionType = thisQuestion.QuestionType
            };

            return PartialView("_EditQuestionPartial");
        }

        public IActionResult CreateEmptyAnswer(int testId, int questionId)
        {
            int answerId = repository.CreateAnswer(questionId);
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        public IActionResult RemoveAnswer(int testId, int questionId, int answerId)
        {
            repository.RemoveAnswerFromQuestion(testId, questionId, answerId);
            return RedirectToAction(nameof(UpdateQuestion), new { testId = testId, questionId = questionId });
        }

        [Route("Admin/Test/{testId}/Question/{questionId}/Update")]
        public IActionResult UpdateQuestion(int testId, int questionId)
        {
            var viewModel = repository.GetEditQuestionVM(testId, questionId);

            return View(viewModel);
        }

        [Route("Admin/Test/{testId}/Settings")]
        public IActionResult EditTestSettings(int testId)
        {
            var output = repository.GetAllTests()
                .Where(o => o.Id == testId)
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
                    CertTemplatePath = model.CertTemplatePath,
                    EnableCertDownloadOnCompletion = model.EnableCertDownloadOnCompletion,
                    EnableEmailCertOnCompletion = model.EnableEmailCertOnCompletion
                })
                .SingleOrDefault();

            return View(output);
        }

        [Route("Admin/Test/{testId}/Settings")]
        [HttpPost]
        public IActionResult EditTestSettings(TestSettingsFormVM viewModel, int? testId)
        {
            if (testId != null)
            {
                var thisTest = repository.GetAllTests().SingleOrDefault(o => o.Id == testId);
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
                thisTest.CertTemplatePath = viewModel.CertTemplatePath;
                thisTest.EnableCertDownloadOnCompletion = viewModel.EnableCertDownloadOnCompletion;
                thisTest.EnableEmailCertOnCompletion = viewModel.EnableEmailCertOnCompletion;
            }
            return RedirectToAction(nameof(AdminController.ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/Create")]
        public IActionResult CreateTest()
        {
            return View();
        }

        [Route("Admin/Test/Create")]
        [HttpPost]
        public IActionResult CreateTest(TestSettingsFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var testId = repository.CreateTest(new Test()
            {
                Name = model.TestName,
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
                CertTemplatePath = model.CertTemplatePath,
                EnableCertDownloadOnCompletion = model.EnableCertDownloadOnCompletion,
                EnableEmailCertOnCompletion = model.EnableEmailCertOnCompletion
            });

            return RedirectToAction(nameof(AdminController.ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/{testId}")]
        public IActionResult ManageTestQuestions(int testId)
        {
            var viewModel = repository.GetManageTestQuestionVM(testId);

            return View(viewModel);
        }

        public IActionResult RemoveQuestion(int testId, int questionId)
        {
            repository.RemoveQuestionFromTest(questionId, testId);
            return RedirectToAction(nameof(ManageTestQuestions), new { testId = testId });
        }

        [Route("Admin/Test/{testId}/Import")]
        public IActionResult Import(int testId)
        {
            var viewModel = new ImportVM()
            {
                TestId = testId
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CopyQuestionsToTest(int testId, int[] questionIds)
        {
            //TODO: multiple questions in one query
            foreach (var qId in questionIds)
                repository.CopyQuestionToTest(qId, testId);

            return Json(GetCurrentTestImportData(testId));
        }

        [HttpPost]
        public IActionResult DeleteQuestionsFromTest(int testId, int[] questionIds)
        {
            //TODO: multiple questions in one query
            foreach (var qId in questionIds)
                repository.RemoveQuestionFromTest(qId, testId);

            return Json(GetCurrentTestImportData(testId));
        }

        public ActionResult PreviewQuestionPartial(int id)
        {
            QuestionFormVM viewModelPartial = repository.GetPreviewQuestionPartial(id);

            return PartialView("_PreviewQuestionFormPartial", viewModelPartial);
        }

        public IActionResult GetImportData(int id)
        {
            var viewModel = new
            {
                allTestsData = GetAllTestsImportData(id),
                currentTestData = GetCurrentTestImportData(id)
            };

            return Json(viewModel);
        }

        
        object GetAllTestsImportData(int currentTestId)
        {
            var allTests = repository.GetAllTests();

            var allTestsData = allTests.Where(t => t.Id != currentTestId).Select(o => new
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

        object GetCurrentTestImportData(int id)
        {
            var allTests = repository.GetAllTests();

            var thisTestData = allTests.Where(o => o.Id == id).Select(o => new
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
            }).Single();
            return thisTestData;
        }

       
    }
}
