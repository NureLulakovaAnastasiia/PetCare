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
        private double serviceInterval = 10;

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
                    foreach(var question in questionaryToAdd)
                    {
                        if(question.Id == 0)
                        {
                            question.ServiceId = service.Id;
                             _dbContext.Add(question);
                            var res = _dbContext.SaveChanges();
                            
                        }
                        else
                        {
                            var answersFromDb = _dbContext.Answers.AsNoTracking().Where(a => a.QuestionId == question.Id).ToList();
                            var answersToDelete = answersFromDb
                                .Where(a => !question.Answers.Any(q => q.Id == a.Id))
                                .ToList(); 
                            foreach(Answer answer in answersToDelete)
                            {
                                _dbContext.Remove(answer);
                            }
                            _dbContext.Update(question);

                        }
                    }
                    
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during adding data";
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

        //returns all working time slots (using schedule and breaks)
        public List<TimeSlot> GetWorkTimeSlots(string masterId)
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

                        if (timeSlots.Where(x => dates.Contains(x.Date.Date)).Count() > 0)
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
                        foreach (var date in dates)
                        {
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

        //return dates for N days ahead(variable daysAhead) for needed day of the week
        private List<DateTime> GetClosestDatesByDayOfWeek(int dayOfWeek)
        {
            var res = new List<DateTime>();
            var now = DateTime.Today;
            while (now <= DateTime.Today.AddDays(daysAhead))
            {
                if ((int)now.DayOfWeek == dayOfWeek)
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

        //final stage of signing up to appointment
        public async Task<string> MakeAppointment(RecordDto recordDto)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                if (recordDto.StartTime > recordDto.EndTime)
                {
                    return "Time interval is not correct";
                }
                var service = _dbContext.Services.FirstOrDefault(x => x.Id == recordDto.ServiceId);
                if (service != null)
                {
                    if (CheckIfFreeTimeSlot(service.AppUserId, recordDto.StartTime, recordDto.EndTime))
                    {
                        var record = _mapper.Map<Record>(recordDto);
                        if (record != null)
                        {
                            record.AppUserId = user.Id;
                            _dbContext.Add(record);
                            return _dbContext.SaveChanges().ToString();
                        }
                    }
                    else
                    {
                        return $"Time from {recordDto.StartTime.ToString("HH:mm")}" +
                            $" to {recordDto.EndTime.ToString("HH:mm")} of {recordDto.StartTime.ToString("ddd MMMM")} " +
                            $"is already booked, choose another";
                    }
                }
                return "Error during adding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //check if there no other appointments at the same time
        private bool CheckIfFreeTimeSlot(string masterId, DateTime startTime, DateTime endTime)
        {
            var intersections = _dbContext.Records.Where(x => x.AppUserId == masterId
                                && x.StartTime > startTime && x.EndTime < endTime
                                && x.Status != "Canceled").ToList();
            if (intersections.Any())
            {
                return false;
            }
            return true;
        }

        //adds to work time slots already signed records(appointments)
        private List<TimeSlot> GetAllEmptySlots(string masterId)
        {
            var resSlots = new List<TimeSlot>();
            var workSlots = GetWorkTimeSlots(masterId);
            var masterServices = _dbContext.Services.Where(x => x.AppUserId == masterId).Select(x => x.Id).ToList();
            if (!masterServices.Any())
            {
                return resSlots;
            }
            var records = _dbContext.Records
                .Where(x => x.Status != "Canceled" &&
                        masterServices.Contains((int)x.ServiceId)
                        && x.StartTime > DateTime.Now)
                .OrderBy(x => x.StartTime)
                .ToList();


            foreach (var slot in workSlots)
            {
                var slotServices = records
                    .Where(x => x.StartTime.Date == slot.Date &&
                    x.StartTime.TimeOfDay >= slot.StartTime &&
                    x.EndTime.TimeOfDay <= slot.EndTime)
                    .OrderBy(x => x.StartTime)
                    .ToList();

                if (slotServices.Count == 0)
                {
                    resSlots.Add(slot);
                }
                else
                {
                    var startTime = slot.StartTime;

                    foreach (var service in slotServices)
                    {
                        if (startTime == service.StartTime.TimeOfDay)
                        {
                            startTime = service.EndTime.TimeOfDay;
                            continue;
                        }
                        resSlots.Add(new TimeSlot
                        {
                            Date = slot.Date,
                            StartTime = startTime,
                            EndTime = service.StartTime.TimeOfDay
                        });
                        startTime = service.EndTime.TimeOfDay;
                    }
                    resSlots.Add(new TimeSlot
                    {
                        Date = slot.Date,
                        StartTime = startTime,
                        EndTime = slot.EndTime
                    });
                }
            }

            return resSlots;
        }

        //creates slots for some amount of time from empty slots for some procedure
        public List<TimeSlot> GetFreeTimeSlots(string masterId, int time, int serviceId)
        {
            var resSlots = new List<TimeSlot>();
            var workSlots = GetAllEmptySlots(masterId);
            var limitations = _dbContext.ServiceLimitations.Where(x => x.ServiceId == serviceId &&
                                                                x.Date > DateTime.Now).ToList();
            var limitDates = limitations.Select(x => x.Date).ToList();
            var limitDayOfWeek = limitations.Select(x => x.DayOfWeek).ToList();
            if (!workSlots.Any())
            {
                return resSlots;
            }
            foreach (var slot in workSlots)
            {
                if (limitDates.Contains(slot.Date.Date) || limitDayOfWeek.Contains((int)slot.Date.DayOfWeek))
                {
                    continue;
                }
                var startTime = slot.StartTime;
                while (startTime < slot.EndTime)
                {
                    var endTime = startTime.Add(TimeSpan.FromMinutes((double)time));
                    if (endTime > slot.EndTime)
                    {
                        break;
                    }
                    resSlots.Add(new TimeSlot
                    {
                        Date = slot.Date,
                        StartTime = startTime,
                        EndTime = endTime
                    });

                    startTime = startTime.Add(TimeSpan.FromMinutes((double)serviceInterval));
                }
            }
            return resSlots;
        }

        public int AnalizeQuestionary(List<QuestionDto> questionary, int serviceId)
        {
            var service = _dbContext.Services.FirstOrDefault(x => x.Id == serviceId);
            if (service == null)
            {
                return 0;
            }
            int resTime = 0;
            var fixedQuestion = questionary.FirstOrDefault(x => x.HasAnswerWithFixedTime);
            if (fixedQuestion != null)
            {
                if (fixedQuestion.Answer.IsTimeMinimum)
                {
                    resTime = service.MinimumTime;
                }
                else if (fixedQuestion.Answer.IsTimeMaximum)
                {
                    resTime = service.MaximumTime;
                }
                else
                {
                    resTime = fixedQuestion.Answer.Time;
                }
                questionary.Remove(fixedQuestion);
            }
            else
            {
                resTime = service.RealTime;
            }

            foreach (var question in questionary)
            {
                resTime += question.Answer.Time;
            }
            return resTime;
        }

        public string GetQuestionaryDescription(List<QuestionDto> questionary)
        {
            string res = "";
            for (int i = 0; i < questionary.Count; i++)
            {
                res += $"{i + 1}. {questionary[i].Name} \n - {questionary[i].Answer.Text}";
            }
            return res;
        }

        public async Task<string> MakeRequestToOrganization(int organizationId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var organization = _dbContext.Organizations.FirstOrDefault(x => x.Id == organizationId);
                if (organization == null)
                {
                    return "No such organization";
                }
                _dbContext.Add(new RequestToOrganization
                {
                    AppUserId = user.Id,
                    OrganizationId = organizationId
                });
                return _dbContext.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> CancelRecord(int recordId, string reason)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var record = _dbContext.Records.Where(x => x.Id == recordId).Include(x => x.Service).FirstOrDefault();
                    if (record == null)
                    {
                        return "There is no such record";
                    }
                    if (record.Service.AppUserId == user.Id &&
                        record.StartTime > DateTime.UtcNow
                        && record.Status == "Created") //check if right master cancels the record
                    {
                        record.Status = "Canceled";
                        _dbContext.Update(record);
                        _dbContext.Add(new RecordCancel
                        {
                            AppUserId = user.Id,
                            RecordId = recordId,
                            Reason = reason,
                            Date = DateTime.UtcNow
                        });
                        _dbContext.SaveChanges();
                    }
                    return "You cannnot cancel this record";
                }
                return "You have no rights to cancel record";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<GetGeneralMasterDto> GetGeneralMasterData()
        {
            var res = new GetGeneralMasterDto();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var userData = _dbContext.Users.Where(x => x.Id == user.Id)
                        .Include(x => x.Services)
                        .Include(x => x.Contacts)
                        .FirstOrDefault();
                    if (userData != null)
                    {
                        res.Contacts = _mapper.Map<GetContactsDto>(userData.Contacts);
                        res.Services = _mapper.Map<List<GetServiceDto>>(userData.Services);
                        res.FirstName = user.FirstName;
                        res.LastName = user.LastName;
                        res.Email = user.Email != null ? user.Email : "";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public async Task<string> UpdateGeneralMasterData(GetGeneralMasterDto masterData)
        {
            var res = "";
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    user.FirstName = !String.IsNullOrEmpty(masterData.FirstName) ? masterData.FirstName : user.FirstName;
                    user.LastName = !String.IsNullOrEmpty(masterData.LastName) ? masterData.LastName : user.LastName;
                    _dbContext.Update(user);
                    var contacts = _mapper.Map<Contacts>(masterData.Contacts);
                    contacts.AppUserId = user.Id;
                    _dbContext.Update(contacts);
                    _dbContext.SaveChanges();
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<UpdateQuestionDto>> GetQuestionary(int serviceId)
        {
            var res = new List<UpdateQuestionDto>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return res;
                }
                var questionary = _dbContext.Questions.Where(s => s.ServiceId == serviceId)
                    .Include(s => s.Answers)
                    .ToList();

                res = QuestionMappings.MapQuestionListToUpdate(questionary);
            }
            catch (Exception ex)
            {
                return res;
            }

            return res;
        }
    }
}
