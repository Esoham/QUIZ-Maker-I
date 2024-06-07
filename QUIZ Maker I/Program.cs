using QuizMaker;
namespace QuizMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var quizManager = new QuizManager();
            var ui = new UI(quizManager);
            ui.Run();
        }
    }
}
