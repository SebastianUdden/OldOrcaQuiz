using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;
using TestPlatform.Repositories;

namespace TestPlatform.Utils
{
    public static class GradeUtils
    {
        public static bool CheckHasPassed(TestSession testSession, int? passPercentage, QuestionsAndAnswersUtils questionsAndAnswers)
        {
            if (!passPercentage.HasValue)
            {
                return true;
            }
            else
            {
                var testScore = TestSessionUtils.GetScore(testSession, questionsAndAnswers.Answers, questionsAndAnswers.Questions);
                var passScore = (((double)passPercentage.Value / 100) * testSession.QuestionResults.Count);
                System.Diagnostics.Debug.WriteLine(testScore);
                System.Diagnostics.Debug.WriteLine(passPercentage.Value / 100);
                System.Diagnostics.Debug.WriteLine(testSession.QuestionResults.Count);
                System.Diagnostics.Debug.WriteLine(passScore);
                if ( passScore <= testScore)
                    return true;
                else
                    return false;
            }
        }


    }
}
