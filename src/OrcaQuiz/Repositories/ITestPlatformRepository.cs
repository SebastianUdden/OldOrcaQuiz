using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Models.Enums;
using TestPlatform.Utils;
using TestPlatform.ViewModels;

namespace TestPlatform.Repositories
{
    public interface ITestPlatformRepository
    {
        #region Module
        List<Module> GetAllModules();
        ManageModuleTestsVM GetManageModuleTestVM(int moduleId);
        Module GetModuleById(int Id);
        void CopyTestToModule(int testId, int moduleId);
        void RemoveTestFromModule(int testId, int moduleId);
        #endregion

        #region Test
        Test[] GetAllTests();
        int CreateTest(Test test);
        void CopyQuestionToTest(int questionId, int testId);
        void RemoveQuestionFromTest(int questionId, int testId);
        #endregion

        #region Question
        Question[] GetAllQuestions();
        int CreateTestQuestion(int testId);
        EditQuestionVM GetEditQuestionVM(int testId, int questionId);
        QuestionFormVM GetPreviewQuestionPartial(int questionId);
        QuestionFormVM GetPreviewQuestion(int questionId);
        ManageTestQuestionsVM GetManageTestQuestionVM(int testId);
        ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession);
        #endregion

        #region Answer
        Answer[] GetAllAnswers();
        int CreateAnswer(int questionId);
        int CreateAnswer(int questionId, AnswerDetailVM viewModel);
        void RemoveAnswerFromQuestion(int testId, int questionId, int answerId);
        #endregion

        #region TestSession
        bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment);
        SessionIndexVM GetSessionIndexVM(int testId);
        int StartNewSession(int userId, int testId);
        void SubmitTestSession(int testSessionId);
        TestSession GetTestSessionById(int testSessionId);
        SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason);
        #endregion

        #region Result
        PdfSymbols GetCertificateSymbols(int testSessionId);
        ShowResultsVM GetShowResultsVM(int testId);
        #endregion
    }
}
