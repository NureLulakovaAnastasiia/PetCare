using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Mappings
{
    public static class QuestionMappings
    {
        
        public static List<Question> MapAddQuestionList(List<AddQuestionDto> questionary, int serviceId)
        {
            var res = new List<Question>();
            foreach (var questionDto in questionary)
            {
                var question = new Question
                {
                    Name = questionDto.Name,
                    HasAnswerWithFixedTime = questionDto.HasAnswerWithFixedTime,
                    ServiceId = serviceId
                };
                foreach (AddAnswerDto answerDto in questionDto.Answers)
                {
                    question.Answers.Add(new Answer
                    {
                        Text = answerDto.Text,
                        Photo = answerDto.Photo,
                        Time = answerDto.Time,
                        IsTimeFixed = answerDto.IsTimeFixed,
                        IsTimeMaximum = answerDto.IsTimeMaximum,
                        IsTimeMinimum = answerDto.IsTimeMinimum
                    });
                }
                res.Add(question);
            }

            return res;
        }

        public static List<Question> MapUpdateQuestionList(List<UpdateQuestionDto> questionary)
        {
            var res = new List<Question>();
            foreach (var questionDto in questionary)
            {
                var question = new Question
                {
                    Id = questionDto.Id,
                    Name = questionDto.Name,
                    HasAnswerWithFixedTime = questionDto.HasAnswerWithFixedTime,
                    ServiceId = questionDto.ServiceId
                };
                foreach (UpdateAnswerDto answerDto in questionDto.Answers)
                {
                    question.Answers.Add(new Answer
                    {
                        Id = answerDto.Id,
                        Text = answerDto.Text,
                        Photo = answerDto.Photo,
                        Time = answerDto.Time,
                        IsTimeFixed = answerDto.IsTimeFixed,
                        IsTimeMaximum = answerDto.IsTimeMaximum,
                        IsTimeMinimum = answerDto.IsTimeMinimum
                    });
                }
                res.Add(question);
            }

            return res;
        }
    }
}
