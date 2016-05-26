using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models;
using OrcaQuiz.Models.Enums;

namespace OrcaQuiz.Utils
{
    public class TestSessionUtils
    {
        internal static int GetScore(TestSession ts, Answer[] _answers, Question[] _questions)
        {
            // Listan correctAnswers motsvarar antalet poäng (1 max per fråga) på provet
            // Int i listan motsvarar antalet rätta svar per fråga
            //correctAnswerCount += _answers.Where(o=>o.QuestionId == question.QuestionId)
            //    .Count(o => o.IsCorrect == true);

            // Initierar variabler
            var correctSelectedAnswerList = new List<int>();
            var maxTestScore = new List<int>();
            int userTestScore = 0;

            // Facit-del
            #region Facit
            // Tar ut alla frågor relaterade till testet
            var testQuestions = _questions.Where(o => o.TestId == ts.TestId).ToList();

            foreach (var question in testQuestions)
            {
                switch (question.QuestionType)
                {
                    case QuestionType.MultipleChoice:
                        userTestScore += MultipleChoiceCorrectQuestions(question, ts);
                        break;
                    case QuestionType.SingleChoice:
                        userTestScore += SingleChoiceCorrectQuestions(question, ts);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("You have to correct the answers manually, fool!");
                        break;
                }
                //if (question.QuestionType == Models.Enums.QuestionType.MultipleChoice)
                //{
                //    userTestScore += MultipleChoiceCorrectQuestions(question, ts);
                //}
                //else if (question.QuestionType == Models.Enums.QuestionType.SingleChoice)
                //{
                //    userTestScore += SingleChoiceCorrectQuestions(question, ts);
                //}
                //else
                //{
                //    System.Diagnostics.Debug.WriteLine("You have to correct the answers manually, fool!");
                //}
            }


            //    //Beräknar antalet rätta svar per fråga
            //    correctQuestionAnswerCount = question.Answers.Count(o => o.IsCorrect == true);
            //    maxTestScore.Add(correctQuestionAnswerCount);
            //}
            //#endregion
            //#region User Answers
            //// Loopar igenom frågorna för nuvarande testsession
            //foreach (var questionResult in ts.QuestionResults)
            //{
            //    // Splitta användarens svar till en array
            //    var selectedAnswers = questionResult.SelectedAnswers.Split(',');

            //    // Kolla varje svar och se om det är korrekt, lägg till 1 på correctSelectedAnswerCount
            //    foreach (var answer in selectedAnswers)
            //    {
            //        var selectedAnswerId = Convert.ToInt32(answer);
            //        if (_answers.Single(o => o.Id == selectedAnswerId).IsCorrect)
            //            correctSelectedAnswerCount = 1;
            //        else
            //            correctSelectedAnswerCount = 0;
            //    }
            //    correctSelectedAnswerList.Add(correctSelectedAnswerCount);
            //    correctSelectedAnswerCount = 0;
            //}
            //#endregion

            //#region Rättning
            //for (int i = 0; i < maxTestScore.Count; i++)
            //{
            //    if (maxTestScore[i] == correctSelectedAnswerList[i])
            //    {
            //        System.Diagnostics.Debug.WriteLine(correctSelectedAnswerList[i]);
            //        System.Diagnostics.Debug.WriteLine(maxTestScore[i]);
            //        testScore.Add(correctSelectedAnswerList[i]);
            //    }
            //}

            System.Diagnostics.Debug.WriteLine(userTestScore);
            #endregion
            return userTestScore;

            // Svarslistan skapas i TestPlatformRepository UpdateSessionAnswers, Kommaseparerad lista

            // Kolla allas svar och se om de är rätt/fel. Slå ihop antal rätt.
            // Flervalsfrågor, endast de korrekta svaren måste vara icheckade.
            // Textsvar, manuell rättning med boolean som false ursprungligen

            // Vi tar in tesstsessionen, kollar de valda svaren per fråga.
            // Kollar 
        }

        private static int MultipleChoiceCorrectQuestions(Question question, TestSession ts)
        {
            int questionScore = 0;
            // Kollar varje fråga i testet och se om användaren svarat rätt
            foreach (var questionResult in ts.QuestionResults)
            {
                if (questionResult.QuestionId == question.Id)
                {
                    var selectedAnswers = questionResult.SelectedAnswers.Split(',');
                    int correctAnswerCount = 0;
                    int correctSelectedAnswerCount = 0;

                    foreach (var answer in question.Answers)
                    {
                        if (answer.IsCorrect)
                        {
                            correctAnswerCount++;

                            foreach (var selectedAnswer in selectedAnswers)
                            {
                                var selectedAnswerId = Convert.ToInt32(selectedAnswer);

                                if (selectedAnswerId == answer.Id)
                                    correctSelectedAnswerCount++;
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(correctAnswerCount);
                    System.Diagnostics.Debug.WriteLine(correctSelectedAnswerCount);

                    if (correctAnswerCount == correctSelectedAnswerCount)
                        questionScore++;
                }
            }
            return questionScore;
        }

        private static int SingleChoiceCorrectQuestions(Question question, TestSession ts)
        {
            int questionScore = 0;
            // Kollar varje fråga i testet och se om användaren svarat rätt
            foreach (var questionResult in ts.QuestionResults)
            {
                if (questionResult.QuestionId == question.Id)
                {
                    var selectedAnswers = questionResult.SelectedAnswers.Split(',');
                    //foreach (var item in selectedAnswers)
                    //{
                    //    System.Diagnostics.Debug.WriteLine(item);
                    //}

                    //System.Diagnostics.Debug.WriteLine(selectedAnswers[0]);

                    foreach (var answer in question.Answers)
                    {
                        if (answer.IsCorrect)
                        {
                            foreach (var selectedAnswer in selectedAnswers)
                            {
                                //System.Diagnostics.Debug.WriteLine(selectedAnswer);
                                var selectedAnswerId = Convert.ToInt32(selectedAnswer);
                                //System.Diagnostics.Debug.WriteLine(selectedAnswerId);
                                //System.Diagnostics.Debug.WriteLine(answer.Id);

                                if (selectedAnswerId == answer.Id)
                                {
                                    questionScore++;
                                }
                            }
                        }
                    }
                }
            }
            return questionScore;
        }
    }
}
