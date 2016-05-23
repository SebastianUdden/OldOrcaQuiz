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
    public class DbRepository : IOrcaQuizRepository
    {
        OrcaQuizContext context;
        public DbRepository(OrcaQuizContext context)
        {
            this.context = context;
        }
        public void CopyQuestionToTest(int questionId, int testId)
        {
            throw new NotImplementedException();
        }

        public void CopyTestToModule(int testId, int moduleId)
        {
            throw new NotImplementedException();
        }

        public int CreateAnswer(int questionId)
        {
            throw new NotImplementedException();
        }

        public int CreateAnswer(int questionId, AnswerDetailVM viewModel)
        {
            throw new NotImplementedException();
        }

        public int CreateTest(Test test)
        {
            throw new NotImplementedException();
        }

        public int CreateTestQuestion(int testId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public PdfSymbols GetCertificateSymbols(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public EditQuestionVM GetEditQuestionVM(int testId, int questionId)
        {
            throw new NotImplementedException();
        }

        public ManageModuleTestsVM GetManageModuleTestVM(int moduleId)
        {
            throw new NotImplementedException();
        }

        public ManageTestQuestionsVM GetManageTestQuestionVM(int testId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public SessionCompletedVM GetSessionCompletedVM(int testSessionId, SessionCompletedReason sessionCompletedReason)
        {
            throw new NotImplementedException();
        }

        public SessionIndexVM GetSessionIndexVM(int testId)
        {
            throw new NotImplementedException();
        }

        public ShowResultsVM GetShowResultsVM(int testId)
        {
            throw new NotImplementedException();
        }

        public TestSession GetTestSessionById(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public ViewQuestionVM GetViewQuestion(int testSessionId, int questionIndex, bool isInSession)
        {
            throw new NotImplementedException();
        }

        public void RemoveAnswerFromQuestion(int testId, int questionId, int answerId)
        {
            throw new NotImplementedException();
        }

        public void RemoveQuestionFromTest(int questionId, int testId)
        {
            throw new NotImplementedException();
        }

        public void RemoveTestFromModule(int testId, int moduleId)
        {
            throw new NotImplementedException();
        }

        public int StartNewSession(int userId, int testId)
        {
            throw new NotImplementedException();
        }

        public void SubmitTestSession(int testSessionId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSessionAnswers(int testSessionId, int questionIndex, string[] selectedAnswers, string comment)
        {
            throw new NotImplementedException();
        }
    }
}
