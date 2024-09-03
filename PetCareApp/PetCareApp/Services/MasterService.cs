using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class MasterService : BaseService, IMasterService
    {
       
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;

        public MasterService(UserManager<AppUser> userManager, IMapper mapper, ApplicationDBContext context,
           IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
        {
            _mapper = mapper;
            _dbContext = context;
        }

        public async Task<string> AddContacts(AddContactsDto contacts)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var resContacts = _mapper.Map<Contacts>(contacts);
                if (resContacts != null)
                {
                    resContacts.AppUserId = user.Id;
                    _dbContext.Add(resContacts);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during saving";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AddQuestionary(List<AddQuestionDto> questionary, int serviceId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                if (service == null)
                {
                    return "Service is not found";
                }
                var checkRes = CheckQuestionary<AddQuestionDto, AddAnswerDto>(questionary);
                if(!String.Equals(checkRes, "Success"))
                {
                    return checkRes;
                }

                var questionaryToAdd = QuestionMappings.MapAddQuestionList(questionary, serviceId);
                if (questionaryToAdd != null)
                {
                    _dbContext.AddRange(questionaryToAdd);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during adding data";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
        
        public string CheckQuestionary<TQuestion, TAnswer>(List<TQuestion> questionary)
            where TQuestion : IQuestionDto<TAnswer>
            where TAnswer : IAnswerDto
        {
            var fixedTimeQuestions = questionary.Where(x => x.HasAnswerWithFixedTime).ToList();
            if (fixedTimeQuestions.Count() > 1)
            {
                return "Questionary cannot have multiple questions with fixed time";
            }
            if (fixedTimeQuestions.Count > 0)
            {
                var fixedAnswers = fixedTimeQuestions[0].Answers
                    .All(x => x.IsTimeFixed && (x.IsTimeMaximum || x.IsTimeMinimum || x.Time > 0));
                if (!fixedAnswers)
                {
                    return "Every answer in question with fixed time must have fixed time";
                }
            }

            foreach (var question in questionary)
            {
                if (question.Answers.Count <= 1)
                {
                    return $"Question \"{question.Name}\" must have at list 2 answers";
                }
            }

            return "Success";
        }
        
        public async Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionary)
        {
            try
            {
                if (questionary.Count == 0)
                {
                    return "Cannot update questionary because its empty";
                }
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == questionary[0].ServiceId);
                if (service == null)
                {
                    return "Service is not found";
                }
                var checkRes = CheckQuestionary<UpdateQuestionDto, UpdateAnswerDto>(questionary);
                if (!String.Equals(checkRes, "Success"))
                {
                    return checkRes;
                }

                var questionaryToAdd = QuestionMappings.MapUpdateQuestionList(questionary);
                if (questionaryToAdd != null)
                {
                    _dbContext.UpdateRange(questionaryToAdd);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during adding data";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> AddService(AddServiceDto serviceDto)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var service  = _mapper.Map<Models.Service>(serviceDto);
                if (service != null)
                {
                    service.AppUserId = user.Id;
                    _dbContext.Add(service);
                    await _dbContext.SaveChangesAsync();
                    return service.Id.ToString();
                }
                return "Error during adding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

       

        public async Task<string> UpdateContacts(ContactsDto contacts)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var resContacts = _mapper.Map<Contacts>(contacts);
                if (resContacts != null)
                {
                    resContacts.AppUserId = user.Id;
                    _dbContext.Update(resContacts);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

       
    }
}
