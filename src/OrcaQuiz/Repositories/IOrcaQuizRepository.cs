using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models;
using OrcaQuiz.Models.Enums;
using OrcaQuiz.Utils;
using OrcaQuiz.ViewModels;

namespace OrcaQuiz.Repositories
{
    public interface IOrcaQuizRepository
    {
        #region Module
        void CreateNewModule(ModuleVM model);
        ManageModuleTestsVM GetManageModuleTestVM(int moduleId);
        ModuleVM[] GetAllModules();
        ModuleVM GetModuleVMByModuleId(int moduleId);
        void EditModule(ModuleVM model);
        void CopyTestToModule(int testId, int moduleId);
        void RemoveTestFromModule(int testId, int moduleId);
        #endregion

        #region Test
        int CreateTest(TestSettingsFormVM test, string username);
        void CopyQuestionToTest(int questionId, int testId, string userName);
        object GetAllTestsImportData(int currentTestId);
        object GetCurrentTestImportData(int id);
        void RemoveQuestionFromTest(int questionId, int testId);
        TestSettingsFormVM GetTestSettingsFormVM(int testId);
        void EditTestSettings(TestSettingsFormVM viewModel, int testId);
        #endregion

        #region Question
        void EditQuestion(int testId, int questionId, EditQuestionVM viewModel);
        int CreateTestQuestion(int testId);
        Task<DashboardVM> GetDashboardVM(string username);
        EditQuestionVM GetEditQuestionVM(int testId, int questionId);
        QuestionFormVM GetPreviewQuestionPartial(int questionId);
        QuestionFormVM GetPreviewQuestion(int questionId);
        ManageTestQuestionsVM GetManageTestQuestionVM(int testId);
        ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession);
        #endregion

        #region Answer
        AnswerDetailVM EditAnswer(int questionId, int answerId, string answerText, int sortOrder, bool isCorrect);
        int CreateAnswer(int questionId);
        int CreateAnswer(int questionId, AnswerDetailVM viewModel);
        void RemoveAnswerFromQuestion(int testId, int questionId, int answerId);
        #endregion

        #region TestSession
        bool EditSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment);
        SessionIndexVM GetSessionIndexVM(int testId, string userName);
        Task<int> StartNewSession(string userName, int testId);
        void SubmitTestSession(int testSessionId);
        SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason);
        #endregion

        #region Result
        PdfSymbols GetCertificateSymbols(int testSessionId);
        ShowResultsVM GetShowResultsVM(int testId);
        #endregion

        //#region User
        // Create user etc..
        //#endregion
    }
}
