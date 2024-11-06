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
                question.Answers = new List<Answer>(); 
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
                        IsTimeMinimum = answerDto.IsTimeMinimum,
                        QuestionId = question.Id
                    });
                }
                res.Add(question);
            }

            return res;
        }

        public static List<UpdateQuestionDto> MapQuestionListToUpdate(List<Question> questionary)
        {
            var res = new List<UpdateQuestionDto>();
            foreach (var question in questionary)
            {
                var questionDto = new UpdateQuestionDto
                {
                    Id = question.Id,
                    Name = question.Name,
                    HasAnswerWithFixedTime = question.HasAnswerWithFixedTime,
                    ServiceId = question.ServiceId != null ? (int)question.ServiceId : 0
                };
                questionDto.Answers = new List<UpdateAnswerDto>();

                foreach (Answer answer in question.Answers)
                {
                    questionDto.Answers.Add(new UpdateAnswerDto
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        Photo = answer.Photo,
                        Time = answer.Time,
                        IsTimeFixed = answer.IsTimeFixed,
                        IsTimeMaximum = answer.IsTimeMaximum,
                        IsTimeMinimum = answer.IsTimeMinimum
                    });
                }
                res.Add(questionDto);
            }

            return res;
        }
    }
}
