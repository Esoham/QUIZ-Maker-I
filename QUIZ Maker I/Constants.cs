namespace QuizMaker
{
    public static class Constants
    {
        public const string DefaultFilePath = "questions.json";
        public const string InvalidOptionMessage = "Invalid option. Please choose again.";
        public const string EnterQuestionPrompt = "Enter the question: ";
        public const string EnterSubjectPrompt = "Enter the subject: ";
        public const string EnterNumberOfChoicesPrompt = "Enter the number of choices: ";
        public const string EnterChoicePrompt = "Enter choice {0}: ";
        public const string EnterCorrectAnswersPrompt = "Enter the correct answer index(es) (comma-separated for multiple answers): ";
        public const string EnterSubjectForQuizPrompt = "Enter the subject for the quiz: ";
        public const string EnterNumberOfQuestionsPrompt = "Enter the number of questions: ";
        public const string NoQuestionsAvailableMessage = "No questions available for the subject: {0}";
        public const string CorrectMessage = "Correct!";
        public const string IncorrectMessage = "Incorrect.";
        public const string FinalScoreMessage = "Your final score is {0} out of {1}.";
        public const int MinQuestions = 10;
        public const int MaxQuestions = 20;
    }
}
