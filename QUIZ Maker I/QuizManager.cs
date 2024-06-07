using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.VisualBasic;
namespace QuizMaker
{
    public class QuizManager
    {
        private List<Question> questions;
        private const string DefaultFilePath = Constants.DefaultFilePath;

        public QuizManager()
        {
            questions = new List<Question>();
            LoadQuestions(DefaultFilePath);
        }

        public QuizManager(string filePath)
        {
            questions = new List<Question>();
            LoadQuestions(filePath);
        }

        public void AddQuestion(Question question)
        {
            questions.Add(question);
            SaveQuestions(DefaultFilePath);
        }

        private void SaveQuestions(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(questions, options);
            File.WriteAllText(filePath, json);
        }

        private void LoadQuestions(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                questions = JsonSerializer.Deserialize<List<Question>>(json) ?? new List<Question>();
            }
        }

        public List<Question> GetQuestionsBySubject(string subject)
        {
            return questions.Where(q => q.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool ValidateAnswers(Question question, List<int> userAnswers)
        {
            return userAnswers.Count == question.CorrectAnswerIndexes.Count &&
                   userAnswers.All(index => question.CorrectAnswerIndexes.Contains(index));
        }
    }
}

