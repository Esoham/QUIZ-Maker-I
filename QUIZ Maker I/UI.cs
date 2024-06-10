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
                DisplayMenuOptions();
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case Constants.MENU_OPTION_ADD_QUESTION:
                        AddQuestion();
                        break;
                    case Constants.MENU_OPTION_CONDUCT_QUIZ:
                        ConductQuiz();
                        break;
                    case Constants.MENU_OPTION_EXIT:
                        running = false;
                        break;
                    default:
                        Console.WriteLine(Constants.INVALID_OPTION_MESSAGE);
                        break;
                }
            }
        }

        private void DisplayMenuOptions()
        {
            Console.WriteLine($"{Constants.MENU_OPTION_ADD_QUESTION}. {Constants.MENU_OPTION_ADD_QUESTION_DESC}");
            Console.WriteLine($"{Constants.MENU_OPTION_CONDUCT_QUIZ}. {Constants.MENU_OPTION_CONDUCT_QUIZ_DESC}");
            Console.WriteLine($"{Constants.MENU_OPTION_EXIT}. {Constants.MENU_OPTION_EXIT_DESC}");
            Console.Write("Choose an option: ");
        }

        private void AddQuestion()
        {
            string questionText;
            do
            {
                questionText = PromptForInput(Constants.ENTER_QUESTION_PROMPT, Constants.QuestionTextEmptyError);
            } while (string.IsNullOrEmpty(questionText));

            string subject;
            do
            {
                subject = PromptForInput(Constants.ENTER_SUBJECT_PROMPT, Constants.SubjectEmptyError);
            } while (string.IsNullOrEmpty(subject));

            int numberOfChoices;
            do
            {
                numberOfChoices = PromptForNumber(Constants.ENTER_NUMBER_OF_CHOICES_PROMPT, Constants.InvalidNumberOfChoicesError);
            } while (numberOfChoices <= 0);

            var choices = GetChoices(numberOfChoices);
            if (choices == null)
            {
                return;
            }

            List<int> correctAnswerIndexes;
            do
            {
                var correctAnswersInput = PromptForInput(Constants.ENTER_CORRECT_ANSWERS_PROMPT, Constants.AtLeastOneCorrectAnswerError);
                correctAnswerIndexes = ParseCorrectAnswers(correctAnswersInput, numberOfChoices);
            } while (correctAnswerIndexes.Count == 0);

            var question = new Question(questionText, choices, correctAnswerIndexes, subject);
            _quizManager.AddQuestion(question);
        }

        private List<string>? GetChoices(int numberOfChoices)
        {
            var choices = new List<string>();
            for (int i = 0; i < numberOfChoices; i++)
            {
                string choice;
                do
                {
                    choice = PromptForInput(string.Format(Constants.ENTER_CHOICE_PROMPT, i + 1), Constants.ChoiceTextEmptyError);
                } while (string.IsNullOrEmpty(choice));

                choices.Add(choice);
            }
            return choices;
        }

        private void ConductQuiz()
        {
            string subject = PromptForSubject();
            int numberOfQuestions = PromptForNumberOfQuestions();

            var questions = _quizManager.GetQuestionsBySubject(subject);
            if (questions.Count == 0)
            {
                Console.WriteLine(string.Format(Constants.NO_QUESTIONS_AVAILABLE_MESSAGE, subject));
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

                string userAnswersInput;
                do
                {
                    userAnswersInput = PromptForInput("Enter your answer(s) (comma-separated for multiple answers): ", Constants.InvalidAnswersError);
                } while (string.IsNullOrEmpty(userAnswersInput));

                var userAnswerIndexes = new List<int>();
                foreach (var answer in userAnswersInput.Split(Constants.SPLIT_SEPARATOR))
                {
                    if (int.TryParse(answer.Trim(), out int answerIndex))
                    {
                        userAnswerIndexes.Add(answerIndex - 1);
                    }
                }

                if (_quizManager.ValidateAnswers(question, userAnswerIndexes))
                {
                    score++;
                    Console.WriteLine(Constants.CORRECT_MESSAGE);
                }
                else
                {
                    Console.WriteLine(Constants.INCORRECT_MESSAGE);
                }
                Console.WriteLine();
            }

            Console.WriteLine(string.Format(Constants.FINAL_SCORE_MESSAGE, score, Math.Min(numberOfQuestions, questions.Count)));
        }

        private string PromptForSubject()
        {
            string? subject;
            do
            {
                subject = PromptForInput(Constants.ENTER_SUBJECT_FOR_QUIZ_PROMPT, Constants.SubjectEmptyError);
            } while (string.IsNullOrEmpty(subject));
            return subject;
        }

        private int PromptForNumberOfQuestions()
        {
            int numberOfQuestions;
            bool validInput;
            do
            {
                Console.Write(Constants.ENTER_NUMBER_OF_QUESTIONS_PROMPT);
                validInput = int.TryParse(Console.ReadLine(), out numberOfQuestions);
                if (!validInput || numberOfQuestions < Constants.MIN_QUESTIONS || numberOfQuestions > Constants.MAX_QUESTIONS)
                {
                    Console.WriteLine($"Number of questions should be between {Constants.MIN_QUESTIONS} and {Constants.MAX_QUESTIONS}.");
                    validInput = false;
                }
            } while (!validInput);
            return numberOfQuestions;
        }

        private string PromptForInput(string prompt, string errorMessage)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(errorMessage);
                }
            } while (string.IsNullOrEmpty(input));
            return input;
        }

        private int PromptForNumber(string prompt, string errorMessage, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            int number;
            bool validInput;
            do
            {
                Console.Write(prompt);
                validInput = int.TryParse(Console.ReadLine(), out number);
                if (!validInput || number < minValue || number > maxValue)
                {
                    Console.WriteLine(errorMessage);
                    validInput = false;
                }
            } while (!validInput);
            return number;
        }

        private List<int> ParseCorrectAnswers(string correctAnswersInput, int numberOfChoices)
        {
            var correctAnswerIndexes = new List<int>();
            foreach (var answer in correctAnswersInput.Split(Constants.SPLIT_SEPARATOR))
            {
                if (int.TryParse(answer.Trim(), out int index) && index > 0 && index <= numberOfChoices)
                {
                    correctAnswerIndexes.Add(index - 1);
                }
                else
                {
                    Console.WriteLine("Invalid answer index. Please enter valid indexes.");
                    return new List<int>();
                }
            }
            return correctAnswerIndexes;
        }
    }
}
