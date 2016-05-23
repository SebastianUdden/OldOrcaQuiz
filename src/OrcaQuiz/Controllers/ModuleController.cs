using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrcaQuiz.Utils;
using OrcaQuiz.ViewModels;
using OrcaQuiz.Repositories;
using OrcaQuiz.Models;

namespace OrcaQuiz.Controllers
{
    public class ModuleController : Controller
    {
        IOrcaQuizRepository repository;

        public ModuleController(IOrcaQuizRepository repository)
        {
            this.repository = repository;
        }

        #region Module
        public IActionResult CreateModule()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateModule(ModuleVM model)
        {
            repository.GetAllModules().Add(new Module
            {
                Id = 10,
                Name = model.Name,
                Description = model.Description,
                Tags = model.Tags
            });
            return RedirectToAction("Modules");
        }

        [Route("Admin/EditModule/{Id}")]
        public IActionResult EditModule(int Id)
        {
            //var modules = repository.GetModules();
            var module = repository.GetModuleById(Id);
            var model = new ModuleVM
            {
                Name = module.Name,
                Description = module.Description,
                Tags = module.Tags,
                Tests = module.Tests
            };

            return View(model);
        }

        [HttpPost]
        [Route("Admin/EditModule/{Id}")]
        public IActionResult EditModule(ModuleVM model)
        {
            var module = repository.GetModuleById(model.Id);
            module.Name = model.Name;
            module.Description = model.Description;
            module.Tags = model.Tags;

            return RedirectToAction("Modules");
            //return RedirectToAction(nameof(AdminController.ManageTestQuestions), new { testId = testId });
        }
        [Route("Admin/Modules")]
        public IActionResult Modules()
        {
            var model = repository.GetAllModules()
                .Select(o => new ModuleVM
                {
                    Name = o.Name,
                    Description = o.Description,
                    Tags = o.Tags,
                    Tests = o.Tests,
                    Id = o.Id
                })
                .ToArray();
            return View(model);
        }

        [Route("Admin/Module/{moduleId}")]
        public IActionResult ManageModuleTests(int moduleId)
        {
            var model = repository.GetManageModuleTestVM(moduleId);
            return View(model);
        }

        object GetAllModulesImportData(int currentModuleId)
        {
            var allModules = repository.GetAllModules();

            var allModulesData = allModules.Where(t => t.Id != currentModuleId).Select(o => new
            {
                text = o.Name,
                children = o.Tests.Select(t => new
                {
                    text = t.Name,
                    children = t.Questions.Select(q => new
                    {
                        id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                        text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                        children = q.Answers.Select(a => new
                        {
                            text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                            state = new { disabled = true }
                        })
                    })
                })
            }).ToArray();

            return allModulesData;
        }

        object GetCurrentModuleImportData(int id)
        {
            var allModules = repository.GetAllModules();

            var thisModuleData = allModules.Where(o => o.Id == id).Select(o => new
            {
                text = o.Name,
                children = o.Tests.Select(t => new
                {
                    text = t.Name,
                    children = t.Questions.Select(q => new
                    {
                        id = $"{AppConstants.Import_QuestionIdPrefix}{q.Id}",
                        text = q.QuestionText.Replace("<iframe", "|FRAME|").Replace("<img", "|IMAGE|").Replace("src", "|SOURCE|"),
                        children = q.Answers.Select(a => new
                        {
                            text = $"{a.AnswerText} {(a.IsCorrect ? " (Correct)" : string.Empty)}",
                            state = new { disabled = true }
                        })
                    }),
                })
            }).Single();
            return thisModuleData;
        }
        #endregion

        #region NewTestStuffWithModule
        [Route("Admin/Module/{moduleId}/Import")]
        public IActionResult ImportModule(int moduleId)
        {
            var viewModel = new ImportModuleVM()
            {
                ModuleId = moduleId
            };

            return View(viewModel);
        }

        public IActionResult GetImportModuleData(int id)
        {
            var viewModel = new
            {
                allModulesData = GetAllModulesImportData(id),
                currentModuleData = GetCurrentModuleImportData(id)
            };

            return Json(viewModel);
        }


        [HttpPost]
        public IActionResult CopyTestsToModule(int moduleId, int[] testIds)
        {
            //TODO: multiple questions in one query
            foreach (var tId in testIds)
                repository.CopyTestToModule(tId, moduleId);

            return Json(GetCurrentModuleImportData(moduleId));
        }

        [HttpPost]
        public IActionResult DeleteTestsFromModule(int moduleId, int[] testIds)
        {
            //TODO: multiple questions in one query
            foreach (var tId in testIds)
                repository.RemoveTestFromModule(tId, moduleId);

            return Json(GetCurrentModuleImportData(moduleId));
        }
        #endregion
    }
}