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
        private int daysAhead = 10;
        
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
                if (!String.Equals(checkRes, "Success"))
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
            if (fixedTimeQuestions.Count > 0 && questionary.GetType() == typeof(List<AddQuestionDto>))
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

                var service = _mapper.Map<Models.Service>(serviceDto);
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

        public async Task<string> UpsertSchedule(List<ScheduleDto> scheduleDto)
        {
            if (scheduleDto.Count == 0)
            {
                return "Nothing to add or update";
            }

            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var schedule = _mapper.Map<List<Schedule>>(scheduleDto);
                if (schedule != null)
                {
                    foreach (var item in schedule)
                    {
                        item.AppUserId = user.Id;
                        if (item.Id == 0)
                        {
                            _dbContext.Add(item);
                        }
                        else
                        {
                            _dbContext.Update(item);
                        }
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating or inserting data";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpsertBreaks(List<BreakDto> breaksDto)
        {
            if (breaksDto.Count == 0)
            {
                return "Nothing to add or update";
            }

            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var breaks = _mapper.Map<List<Break>>(breaksDto);
                if (breaks != null)
                {
                    foreach (var item in breaks)
                    {
                        item.AppUserId = user.Id;
                        if (item.Id == 0)
                        {
                            _dbContext.Add(item);
                        }
                        else
                        {
                            _dbContext.Update(item);
                        }
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating or inserting data";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<TimeSlot> getWorkTimeSlots(string masterId)
        {
            var timeSlots = new List<TimeSlot>();
            var schedule = _dbContext.Schedules.Where(
                x => x.AppUserId == masterId)
                .OrderByDescending(x => x.Date)
                .OrderBy(x => x.DayOfWeek)
                .ToList();
            var breaks = _dbContext.Breaks.Where(x => x.AppUserId == masterId).ToList();

            if (schedule != null && schedule.Count() > 0)
            {
                foreach (var item in schedule)
                {
                    int dayOfWeek = 0;
                    List<DateTime> dates = new List<DateTime>();
                    if (item.DayOfWeek != null)
                    {
                        dates = GetClosestDatesByDayOfWeek(item.DayOfWeek.Value);
                        
                        if (timeSlots.Where(x=> dates.Contains(x.Date.Date)).Count() > 0)
                        {
                            continue;
                        }
                        dayOfWeek = item.DayOfWeek.Value;
                    }
                    else if (item.Date != null)
                    {
                        dates.Add(item.Date.Value);
                        dayOfWeek = (int)item.Date.Value.DayOfWeek;
                    }
                    
                    var dayBreaks = breaks.Where(x => x.DayOfWeek == dayOfWeek)
                       .OrderBy(x => x.StartTime)
                       .ToList();

                    if (dayBreaks.Count() == 0)
                    {
                        foreach (var date in dates) { 
                            timeSlots.Add(new TimeSlot
                            {
                                Date = date,
                                StartTime = item.StartTime,
                                EndTime = item.EndTime
                            });
                        }
                    }
                    else
                    {
                       
                        foreach (var date in dates)
                        {
                            TimeSpan startTime = item.StartTime;
                            for (int i = 0; i < dayBreaks.Count; i++)
                            {
                                timeSlots.Add(new TimeSlot
                                {
                                    Date = date,
                                    StartTime = startTime,
                                    EndTime = dayBreaks[i].StartTime
                                });
                                startTime = dayBreaks[i].EndTime;
                            }
                            timeSlots.Add(new TimeSlot
                            {
                                Date = date,
                                StartTime = startTime,
                                EndTime = item.EndTime
                            });
                        }
                    }
                }

            }

            return timeSlots.OrderBy(x => x.Date).ToList();
        }

        private List<DateTime> GetClosestDatesByDayOfWeek(int dayOfWeek)
        {
            var res = new List<DateTime>();
            var now = DateTime.Today;
            while(now <= DateTime.Today.AddDays(daysAhead))
            {
                if((int)now.DayOfWeek == dayOfWeek)
                {
                    res.Add(now);
                    now = now.AddDays(7);
                }
                else
                {
                    now = now.AddDays(1);
                }
            }

            return res;
        }
    }
}
