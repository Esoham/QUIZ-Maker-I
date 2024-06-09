using System;
using System.Collections.Generic;
namespace QuizMaker
{
    public class UI
    {
        private readonly QuizManager _quizManager;
        private readonly Random _random;

        public UI(QuizManager quizManager)
        {
            _quizManager = quizManager;
            _random = new Random();
        }

        public void Run()
        {
            Console.WriteLine("Quiz Maker");
            bool running = true;
            while (running)
            {
                Console.WriteLine("1. Add a Question");
                Console.WriteLine("2. Take the Quiz");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddQuestion();
                        break;
                    case "2":
                        ConductQuiz();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine(Constants.InvalidOptionMessage);
                        break;
                }
            }
        }

        private void AddQuestion()
        {
            Console.Write(Constants.EnterQuestionPrompt);
            string? questionText = Console.ReadLine();
            if (string.IsNullOrEmpty(questionText))
            {
                Console.WriteLine("Question text cannot be empty.");
                return;
            }

            Console.Write(Constants.EnterSubjectPrompt);
            string? subject = Console.ReadLine();
            if (string.IsNullOrEmpty(subject))
            {
                Console.WriteLine("Subject cannot be empty.");
                return;
            }

            Console.Write(Constants.EnterNumberOfChoicesPrompt);
            if (!int.TryParse(Console.ReadLine(), out int numberOfChoices))
            {
                Console.WriteLine("Invalid number of choices.");
                return;
            }

            var choices = new List<string>();
            for (int i = 0; i < numberOfChoices; i++)
            {
                Console.Write(string.Format(Constants.EnterChoicePrompt, i + 1));
                string? choice = Console.ReadLine();
                if (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Choice text cannot be empty.");
                    return;
                }
                choices.Add(choice);
            }

            Console.Write(Constants.EnterCorrectAnswersPrompt);
            var correctAnswers = Console.ReadLine()?.Split(Constants.SplitSeparator);
            if (correctAnswers == null || correctAnswers.Length == 0)
            {
                Console.WriteLine("At least one correct answer must be provided.");
                return;
            }

            var correctAnswerIndexes = new List<int>();
            foreach (var answer in correctAnswers)
            {
                if (int.TryParse(answer.Trim(), out int index))
                {
                    correctAnswerIndexes.Add(index - 1);
                }
            }

            var question = new Question(questionText, choices, correctAnswerIndexes, subject);
            _quizManager.AddQuestion(question);
        }

        private void ConductQuiz()
        {
            string subject = PromptForSubject();
            int numberOfQuestions = PromptForNumberOfQuestions();

            var questions = _quizManager.GetQuestionsBySubject(subject);
            if (questions.Count == 0)
            {
                Console.WriteLine(string.Format(Constants.NoQuestionsAvailableMessage, subject));
                return;
            }

            int score = 0;
            var usedQuestions = new HashSet<int>();

            for (int i = 0; i < Math.Min(numberOfQuestions, questions.Count); i++)
            {
                int index;
                do
                {
                    index = _random.Next(questions.Count);
                } while (!usedQuestions.Add(index));

                var question = questions[index];
                Console.WriteLine(question.Text);
                for (int j = 0; j < question.Choices.Count; j++)
                {
                    Console.WriteLine($"{j + 1}. {question.Choices[j]}");
                }

                Console.Write("Enter your answer(s) (comma-separated for multiple answers): ");
                var userAnswers = Console.ReadLine()?.Split(Constants.SplitSeparator);
                if (userAnswers == null)
                {
                    Console.WriteLine("Invalid answers.");
                    continue;
                }

                var userAnswerIndexes = new List<int>();
                foreach (var answer in userAnswers)
                {
                    if (int.TryParse(answer.Trim(), out int answerIndex))
                    {
                        userAnswerIndexes.Add(answerIndex - 1);
                    }
                }

                if (_quizManager.ValidateAnswers(question, userAnswerIndexes))
                {
                    score++;
                    Console.WriteLine(Constants.CorrectMessage);
                }
                else
                {
                    Console.WriteLine(Constants.IncorrectMessage);
                }
                Console.WriteLine();
            }

            Console.WriteLine(string.Format(Constants.FinalScoreMessage, score, Math.Min(numberOfQuestions, questions.Count)));
        }

        private string PromptForSubject()
        {
            string? subject;
            do
            {
                Console.Write(Constants.EnterSubjectForQuizPrompt);
                subject = Console.ReadLine();
                if (string.IsNullOrEmpty(subject))
                {
                    Console.WriteLine("Subject cannot be empty.");
                }
            } while (string.IsNullOrEmpty(subject));
            return subject;
        }

        private int PromptForNumberOfQuestions()
        {
            int numberOfQuestions;
            bool validInput;
            do
            {
                Console.Write(Constants.EnterNumberOfQuestionsPrompt);
                validInput = int.TryParse(Console.ReadLine(), out numberOfQuestions);
                if (!validInput || numberOfQuestions < Constants.MinQuestions || numberOfQuestions > Constants.MaxQuestions)
                {
                    Console.WriteLine($"Number of questions should be between {Constants.MinQuestions} and {Constants.MaxQuestions}.");
                    validInput = false;
                }
            } while (!validInput);
            return numberOfQuestions;
        }
    }
}
