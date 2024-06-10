using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
namespace QuizMaker
{
    public class QuizManager
    {
        private List<Question> questions;
        private const string DefaultFilePath = Constants.DEFAULT_FILE_PATH;
        private readonly JsonSerializerOptions _jsonOptions;

        public QuizManager()
        {
            questions = new List<Question>();
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            LoadQuestions(DefaultFilePath);
        }

        public QuizManager(string filePath)
        {
            questions = new List<Question>();
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            LoadQuestions(filePath);
        }

        public void AddQuestion(Question question)
        {
            questions.Add(question);
            SaveQuestions(DefaultFilePath);
        }

        private void SaveQuestions(string filePath)
        {
            try
            {
                var json = JsonSerializer.Serialize(questions, _jsonOptions);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving questions: {ex.Message}");
            }
        }

        private void LoadQuestions(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    questions = JsonSerializer.Deserialize<List<Question>>(json, _jsonOptions) ?? new List<Question>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions: {ex.Message}");
                questions = new List<Question>();
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
