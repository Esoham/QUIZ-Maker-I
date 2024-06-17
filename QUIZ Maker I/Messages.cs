namespace QuizMaker
{
    public static class Messages
    {
        public const string DEFAULT_FILE_PATH = "questions.json";
        public const string INVALID_OPTION_MESSAGE = "Invalid option. Please choose again.";
        public const string ENTER_QUESTION_PROMPT = "Enter the question: ";
        public const string ENTER_SUBJECT_PROMPT = "Enter the subject: ";
        public const string ENTER_NUMBER_OF_CHOICES_PROMPT = "Enter the number of choices: ";
        public const string ENTER_CHOICE_PROMPT = "Enter choice {0}: ";
        public const string ENTER_CORRECT_ANSWERS_PROMPT = "Enter the correct answer index(es) (comma-separated for multiple answers): ";
        public const string ENTER_SUBJECT_FOR_QUIZ_PROMPT = "Enter the subject for the quiz: ";
        public const string ENTER_NUMBER_OF_QUESTIONS_PROMPT = "Enter the number of questions: ";
        public const string NO_QUESTIONS_AVAILABLE_MESSAGE = "No questions available for the subject: {0}";
        public const string CORRECT_MESSAGE = "Correct!";
        public const string INCORRECT_MESSAGE = "Incorrect.";
        public const string FINAL_SCORE_MESSAGE = "Your final score is {0} out of {1}.";

        // Constants for menu descriptions
        public const string MENU_OPTION_ADD_QUESTION_DESC = "Add a Question";
        public const string MENU_OPTION_CONDUCT_QUIZ_DESC = "Take the Quiz";
        public const string MENU_OPTION_EXIT_DESC = "Exit";

        // Error messages for prompts
        public const string QuestionTextEmptyError = "Question text cannot be empty.";
        public const string SubjectEmptyError = "Subject cannot be empty.";
        public const string InvalidNumberOfChoicesError = "Invalid number of choices.";
        public const string ChoiceTextEmptyError = "Choice text cannot be empty.";
        public const string AtLeastOneCorrectAnswerError = "At least one correct answer must be provided.";
        public const string InvalidAnswersError = "Invalid answers.";

        // New constant for add question prompt
        public const string ADD_ANOTHER_QUESTION_PROMPT = "Do you want to add another question? (yes/no): ";

        // New constant for yes response
        public const string YES_RESPONSE = "yes";
    }
}