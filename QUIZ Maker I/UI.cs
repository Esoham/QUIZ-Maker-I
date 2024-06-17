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
                        AddMultipleQuestions();
                        break;
                    case Constants.MENU_OPTION_CONDUCT_QUIZ:
                        ConductQuiz();
                        break;
                    case Constants.MENU_OPTION_EXIT:
                        running = false;
                        break;
                    default:
                        Console.WriteLine(Messages.INVALID_OPTION_MESSAGE);
                        break;
                }
            }
        }

        private void DisplayMenuOptions()
        {
            Console.WriteLine($"{Constants.MENU_OPTION_ADD_QUESTION}. {Messages.MENU_OPTION_ADD_QUESTION_DESC}");
            Console.WriteLine($"{Constants.MENU_OPTION_CONDUCT_QUIZ}. {Messages.MENU_OPTION_CONDUCT_QUIZ_DESC}");
            Console.WriteLine($"{Constants.MENU_OPTION_EXIT}. {Messages.MENU_OPTION_EXIT_DESC}");
            Console.Write("Choose an option: ");
        }

        private void AddMultipleQuestions()
        {
            var questions = new List<Question>();
            string continueAdding;
            do
            {
                questions.Add(AddQuestion());
                Console.Write(Messages.ADD_ANOTHER_QUESTION_PROMPT);
                continueAdding = Console.ReadLine();
            } while (continueAdding?.ToLower() == Messages.YES_RESPONSE);

            foreach (var question in questions)
            {
                _quizManager.AddQuestion(question);
            }
        }

        private Question AddQuestion()
        {
            string questionText = GetValidatedInput(Messages.ENTER_QUESTION_PROMPT, Messages.QuestionTextEmptyError);
            string subject = GetValidatedInput(Messages.ENTER_SUBJECT_PROMPT, Messages.SubjectEmptyError);
            int numberOfChoices = GetValidatedNumber(Messages.ENTER_NUMBER_OF_CHOICES_PROMPT, Messages.InvalidNumberOfChoicesError);

            var choices = GetChoices(numberOfChoices);
            if (choices == null)
            {
                throw new InvalidOperationException("Choices cannot be null.");
            }

            var correctAnswersInput = GetValidatedInput(Messages.ENTER_CORRECT_ANSWERS_PROMPT, Messages.AtLeastOneCorrectAnswerError);
            var correctAnswerIndexes = ParseCorrectAnswers(correctAnswersInput, numberOfChoices);

            return new Question(questionText, choices, correctAnswerIndexes, subject);
        }

        private List<string>? GetChoices(int numberOfChoices)
        {
            var choices = new List<string>();
            for (int i = 0; i < numberOfChoices; i++)
            {
                string choice = GetValidatedInput(string.Format(Messages.ENTER_CHOICE_PROMPT, i + 1), Messages.ChoiceTextEmptyError);
                choices.Add(choice);
            }
            return choices;
        }

        private void ConductQuiz()
        {
            string subject = GetValidatedInput(Messages.ENTER_SUBJECT_FOR_QUIZ_PROMPT, Messages.SubjectEmptyError);
            int numberOfQuestions = GetValidatedNumber(Messages.ENTER_NUMBER_OF_QUESTIONS_PROMPT, Messages.InvalidNumberOfChoicesError, Constants.MIN_QUESTIONS, Constants.MAX_QUESTIONS);

            var questions = _quizManager.GetQuestionsBySubject(subject);
            if (questions.Count == 0)
            {
                Console.WriteLine(string.Format(Messages.NO_QUESTIONS_AVAILABLE_MESSAGE, subject));
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
                DisplayQuestionChoices(question);

                string userAnswersInput = GetValidatedInput("Enter your answer(s) (comma-separated for multiple answers): ", Messages.InvalidAnswersError);
                var userAnswerIndexes = _quizManager.ParseUserAnswers(userAnswersInput);

                if (_quizManager.ValidateAnswers(question, userAnswerIndexes))
                {
                    score++;
                    Console.WriteLine(Messages.CORRECT_MESSAGE);
                }
                else
                {
                    Console.WriteLine(Messages.INCORRECT_MESSAGE);
                }
                Console.WriteLine();
            }

            Console.WriteLine(string.Format(Messages.FINAL_SCORE_MESSAGE, score, Math.Min(numberOfQuestions, questions.Count)));
        }

        private void DisplayQuestionChoices(Question question)
        {
            Console.WriteLine(question.Text);
            for (int j = 0; j < question.Choices.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {question.Choices[j]}");
            }
        }

        private string GetValidatedInput(string prompt, string errorMessage)
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

        private int GetValidatedNumber(string prompt, string errorMessage, int minValue = int.MinValue, int maxValue = int.MaxValue)
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