using System;
using System.Collections.Generic;
namespace QuizMaker
{
    [Serializable]
    public class Question
    {
        public string Text { get; set; }
        public List<string> Choices { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
        public string Subject { get; set; }

        public Question(string text, List<string> choices, List<int> correctAnswerIndexes, string subject)
        {
            Text = text;
            Choices = choices;
            CorrectAnswerIndexes = correctAnswerIndexes;
            Subject = subject;
        }
    }
}
