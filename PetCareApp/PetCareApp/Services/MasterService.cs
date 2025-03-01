using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration.UserSecrets;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

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

        private void AddHistoryEvent(HistoryEvent historyEvent){
            try
            {
                _dbContext.Add(historyEvent);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
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
                    .All(x => x.IsTimeFixed && (x.IsTimeMaximum || x.IsTimeMinimum || x.Time >= 0));
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
                    
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateQuestionaryUpdateEvent(user.Id, service.Name));
                    }
                    return result.ToString();
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

        public async Task<string> UpsertBreaks(List<BreakDto> breaksDto, string? masterId = null)
        {
            if (breaksDto.Count == 0)
            {
                return "Nothing to add or update";
            }

            try
            {
                if (masterId == null)
                {
                    var user = await GetCurrentUserAsync();
                    if (user == null)
                    {
                        return "User not found";
                    }
                    masterId = user.Id;
                }
                var breaks = _mapper.Map<List<Break>>(breaksDto);
                if (breaks != null)
                {
                    var updatedIds = new List<int>();
                    foreach (var item in breaks)
                    {
                        item.AppUserId = masterId;
                        if (updatedIds.Contains(item.Id) || item.Id == 0)
                        {
                            item.Id = 0;
                            _dbContext.Add(item);
                        }
                        else
                        {
                            _dbContext.Update(item);
                            updatedIds.Add(item.Id);
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

        public async Task<string> DeleteBreaks(List<int> breaksId)
        {
            if (breaksId.Count == 0)
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
                    foreach (var id in breaksId)
                    {
                        var itemToDelete = _dbContext.Breaks.FirstOrDefault(b => b.Id == id);
                        if(itemToDelete != null)
                        {
                            _dbContext.Remove(itemToDelete);
                        }
                    }
                    return _dbContext.SaveChanges().ToString();
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
            timeSlots.RemoveAll(s => s.Date < DateTime.Now);
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
                            var result = _dbContext.SaveChanges();
                            if (result > 0)
                            {
                                AddHistoryEvent(HistoryEventFactory.CreateNewAppointmentEvent(user.Id, service.Name, record.StartTime));
                                if (user.Id != service.AppUserId)
                                {
                                    AddHistoryEvent(HistoryEventFactory.CreateNewAppointmentEvent(service.AppUserId, service.Name, record.StartTime));
                                }

                            }
                            return result.ToString();
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
            var services = _dbContext.Services
                .Where(s => s.AppUserId == masterId)
                .Select(s => s.Id).Distinct().ToList();
            var intersections = _dbContext.Records.Where(x => services.Contains(x.ServiceId ?? 0)
                                && x.StartTime.Date == startTime.Date
                                && x.Status != "Cancelled").ToList();
            intersections = intersections
                .Where(x => (endTime > x.StartTime && startTime > x.EndTime)
                || (startTime >= x.StartTime && (endTime >= x.EndTime && endTime != x.StartTime)))
                .ToList();

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
        public List<TimeSlot> GetFreeTimeSlots(int time, int serviceId, string? masterId = null)
        {
            var resSlots = new List<TimeSlot>();
            if (masterId == null)
            {
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                if (service != null)
                {
                    masterId = service.AppUserId;
                }
                else
                {
                    return resSlots;
                }
            }
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


        //creates optimized slots for some amount of time from empty slots for some procedure
        public List<TimeSlot> GetBetterFreeTimeSlots(int time, int serviceId, string? masterId = null)
        {
            var resSlots = new List<TimeSlot>();
            var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);

            if (masterId == null)
            {
                if (service != null)
                {
                    masterId = service.AppUserId;
                }
                else
                {
                    return resSlots;
                }
            }
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
                var difference = slot.EndTime - startTime;
                if(difference.Minutes > service.MinimumTime)
                {
                    continue;
                }
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
                res += $"{i + 1}. {questionary[i].Name}  - {questionary[i].Answer.Text} \n";
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
                var employee = _dbContext.OrganizationEmployees.Where(e => e.AppUserId == user.Id && e.DismissalDate == null).FirstOrDefault();
                if (employee != null)
                {
                    return "You are already part of this organization";

                }

                var request = _dbContext.RequestsToOrganization
                    .Where(r => r.OrganizationId == organizationId && r.AppUserId == user.Id && r.Status == "New")
                    .OrderBy(r => r.Date)
                    .FirstOrDefault();
                if (request == null)
                {
                    _dbContext.Add(new RequestToOrganization
                    {
                        AppUserId = user.Id,
                        OrganizationId = organizationId
                    });
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateNewOrgRequestEvent(user.Id, organization.Name));
                        AddHistoryEvent(HistoryEventFactory.CreateNewOrgRequestOrgEvent(organization.AppUserId, user.FirstName + user.LastName));

                    }
                    return result.ToString();
                }
                else
                {
                    return "You've already made request to this organization";
                }
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
                        record.Status = "Cancelled";
                        _dbContext.Update(record);
                        _dbContext.Add(new RecordCancel
                        {
                            AppUserId = user.Id,
                            RecordId = recordId,
                            Reason = reason,
                            Date = DateTime.UtcNow
                        });
                        var result = _dbContext.SaveChanges();
                        if (result > 0)
                        {
                            AddHistoryEvent(HistoryEventFactory.CreateRecordCancellationEvent(user.Id, record.Service.Name, record.StartTime));
                        }
                        return result.ToString();
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

        public async Task<string> UpdateAppointments(List<GetRecordDto> recordsDto)
        {
            try
            {
                if (recordsDto.Count == 0)
                {
                    return "No data to update";
                }
                var res = "";
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    foreach (var rec in recordsDto)
                    {
                        var service = _dbContext.Services.Where(s => s.Id == recordsDto[0].ServiceId).FirstOrDefault();
                        if (service != null)
                        {
                            var masterServices = _dbContext.Services.Where(s => s.AppUserId == user.Id)
                                .Select(s => s.Id).ToList();

                            var hasOverlap = CheckIfFreeTimeSlot(service.AppUserId, rec.StartTime, rec.EndTime);
                            if (hasOverlap)
                            {
                                return $"Records is already set to this time ({rec.StartTime})";
                            }

                            var record = _dbContext.Records.FirstOrDefault(r => r.Id == rec.Id);
                            if (record != null)
                            {
                                record.StartTime = rec.StartTime;
                                record.EndTime = rec.EndTime;
                                record.Status = rec.Status;
                                record.Description = rec.Description;
                                record.ServiceId = rec.ServiceId;
                                record.Comment = rec.Comment;
                                _dbContext.Update(record);
                                return _dbContext.SaveChanges().ToString();
                            }
                            res = $"There is no such record {rec.Id}";
                        }
                        else
                        {
                            return "There is no service like this";
                        }
                    }
                }
                return "Unauthorized";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        //!!!! make localization !!!!
        
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
                        .Include(x => x.Pets)
                        .Include(x => x.Contacts)
                        .ThenInclude(c => c.Location)
                        .FirstOrDefault();
                    if (userData != null)
                    {
                        res.Contacts = ContactsMapping.MapContact(userData.Contacts);
                        if(res.Contacts.AppUserId == null)
                        {
                            res.Contacts.AppUserId = user.Id;
                        }
                        var city = _dbContext.Cities.FirstOrDefault(c => c.Id == userData.Contacts.CityId);
                        if (city != null)
                        {
                            var localized = JsonSerializer.Deserialize<Dictionary<string, string>>(city.LocalizedName);
                            if (localized != null)
                            {
                                res.Contacts.City = localized["en"] ?? string.Empty;
                            }
                        }
                        res.Pets = _mapper.Map<List<PetDto>>(userData.Pets);
                        res.Services = _mapper.Map<List<GetServiceDto>>(userData.Services);
                        res.FirstName = user.FirstName;
                        res.LastName = user.LastName;
                        res.Email = user.Email != null ? user.Email : "";
                        res.Photo = user.Photo;
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
                    user.Photo = masterData.Photo;
                    _dbContext.Update(user);
                    var contacts = ContactsMapping.MapFromDto(masterData.Contacts);
                    
                    //find city
                    contacts.CityId = findCity(contacts.Location, masterData.Contacts.City);
                    var location = _dbContext.Locations.AsNoTracking().FirstOrDefault(x => x.ContactsId == contacts.Id);
                    if (location != null)
                    {
                        contacts.Location.Id = location.Id;
                    }
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


        private int findCity(Location location, string city)
        {
            var searchedCity = _dbContext.Cities
            .FirstOrDefault(c => c.LocalizedName.Contains(city));

            return searchedCity == null ? 0 : searchedCity.Id ;
        }

        private double DegToRad(double deg) => deg * (Math.PI / 180);

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
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                if(service == null || service.AppUserId != user.Id)
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

        public async Task<string> DeleteQuestionary(int serviceId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User is not found";
                }
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                if (service == null || service.AppUserId != user.Id)
                {
                    return "You don't have rights to delete this questionary";
                }
                var questions = _dbContext.Questions.Where(q => q.ServiceId == serviceId).ToList();
                if (questions.Count > 0)
                {
                    _dbContext.RemoveRange(questions);
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateQuestionaryDeleteEvent(user.Id, service.Name));
                    }
                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Error during deleting questionary";
        }

        public async Task<Result<List<GetRecordDto>>> GetRecordsForMonth(int month, int year, string? masterId)
        {
            var res = new Result<List<GetRecordDto>>();    
            try
            {
                if (masterId == null)
                {
                    var user = await GetCurrentUserAsync();
                    if (user == null)
                    {
                        res.ErrorMessage = "User not found";
                        return res;
                    }
                    masterId = user.Id;
                }
                var services = _dbContext.Services.Where(s => s.AppUserId == masterId).ToList();
                var servicesIds = services.Select(s => s.Id).ToList();
                if (services == null || !services.Any())
                {
                    res.ErrorMessage = "Master has no services";
                    return res;
                }

                var records = _dbContext.Records
                    .Where(r => servicesIds.Contains((int)r.ServiceId) && 
                    (r.StartTime.Month == month && r.StartTime.Year == year))
                    .ToList();
                if (records.Count > 0)
                {
                    res.Data = _mapper.Map<List<GetRecordDto>>(records);
                    foreach (var record in res.Data)
                    {
                        record.ServiceName = services.Where(s => s.Id == record.ServiceId).Select(s => s.Name).First();
                    }
                }
                else
                {
                    res.ErrorMessage = "No records were found";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
                return res;
            }

            return res;
        }

        public async Task<Result<List<BreakDto>>> GetMasterBreaks(string? masterId)
        {
            var res = new Result<List<BreakDto>>();
            try
            {
                var user = new AppUser();
                if (masterId == null)
                {
                    user = await GetCurrentUserAsync();
                }
                else
                {
                    user = _dbContext.Users.FirstOrDefault(u => u.Id == masterId);
                }
                if (user == null)
                {
                    res.ErrorMessage = "User not found";
                    return res;
                }
                var breaks = _dbContext.Breaks.Where(x => x.AppUserId == user.Id).ToList();
                if (breaks == null || breaks.Count == 0)
                {
                    res.ErrorMessage = "No breaks found";
                }
                else
                {
                    res.Data = _mapper.Map<List<BreakDto>>(breaks);
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
                return res;
            }

            return res;
        }

        public async Task<Result<List<PortfolioDto>>> GetMasterPortfolio(string? masterId)
        {
            var res = new Result<List<PortfolioDto>>();
            try
            {
                
                var user = await GetCurrentUserAsync();
                if (masterId == null )
                {
                    if (user == null)
                    {
                        res.ErrorMessage = "User not found";
                        return res;
                    }
                }
                var portfolio = _dbContext.Portfolios.Where(x => x.AppUserId == (masterId == null ? user.Id : masterId)).ToList();
                if (portfolio == null || portfolio.Count == 0)
                {
                    res.ErrorMessage = "No portfolio were found";
                }
                else
                {
                    res.Data = _mapper.Map<List<PortfolioDto>>(portfolio);
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
                return res;
            }

            return res;
        }

        public async Task<Result<List<int>>> UpsertPortfolio(List<PortfolioDto> portfolio)
        {
            var res = new Result<List<int>>();
            if (portfolio.Count == 0)
            {
                res.ErrorMessage = "Nothing to add or update";
            }

            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "User not found";
                    return res;
                }

                var addPortfolio = _mapper.Map<List<Portfolio>>(portfolio);
                if (addPortfolio != null)
                {
                    res.Data = new List<int>();
                    foreach (var item in addPortfolio)
                    {
                        item.AppUserId = user.Id;
                        if (item.Id == 0)
                        {
                            _dbContext.Add(item);
                            res.ErrorMessage = _dbContext.SaveChanges().ToString();
                            res.Data.Add(item.Id);
                        }
                        else
                        {
                            _dbContext.Update(item);
                        }
                    }
                    res.ErrorMessage = _dbContext.SaveChanges().ToString();
                }
                else
                {
                    res.ErrorMessage = "Error during updating or inserting data";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<string> DeletePortfolio(int portfolioId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User is not found";
                }
                var porfolio = _dbContext.Portfolios.FirstOrDefault(s => s.Id == portfolioId);
                if (porfolio == null || porfolio.AppUserId != user.Id)
                {
                    return "You don't have rights to delete this item";
                }
                
                    _dbContext.Remove(porfolio);
                var orgPortfolios = _dbContext.OrganizationPorfolios
                        .Where(p => p.PortfolioId == portfolioId)
                        .ToList();

                if (orgPortfolios != null)
                {
                    _dbContext.RemoveRange(orgPortfolios);
                }
                return _dbContext.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Result<List<GetServiceDto>>> GetMasterServices(string? masterId)
        {
            var res = new Result<List<GetServiceDto>>();
            try
            {

                var user = await GetCurrentUserAsync();
                if (masterId == null)
                {
                    if (user == null)
                    {
                        res.ErrorMessage = "User not found";
                        return res;
                    }
                }
                var services = _dbContext.Services
                    .Where(s => s.AppUserId == (masterId == null ? user.Id : masterId))
                    .Include(s => s.Reviews)
                    .ToList();
                if (services == null || services.Count == 0)
                {
                    res.ErrorMessage = "No services were found";
                }
                else
                {
                    res.Data = _mapper.Map<List<GetServiceDto>>(services);
                    foreach (var service in res.Data)
                    {
                        if (service.Reviews.Count > 0)
                        {
                            service.Rate = service.Reviews.Average(r => r.Rate);
                            service.Reviews.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
                return res;
            }

            return res;
        }

        public Result<List<GetQuestionDto>> GetQuestionaryForUser(int serviceId)
        {
            var res = new Result<List<GetQuestionDto>>();
            try
            {
                
                var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                if (service == null)
                {
                    res.ErrorMessage = "No such service were found";
                }
                var questionary = _dbContext.Questions.Where(s => s.ServiceId == serviceId)
                    .Include(s => s.Answers)
                    .ToList();

                res.Data = QuestionMappings.MapUserQuestionary(questionary);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<List<ScheduleDto>>> GetMasterSchedule(string? masterId)
        {
            var res = new Result<List<ScheduleDto>>();

            try
            {
                if (masterId == null)
                {
                    var user = await GetCurrentUserAsync();
                    if (user == null)
                    {
                        res.ErrorMessage = "User was not found";
                        return res;
                    }
                    else
                    {
                        masterId = user.Id;
                    }
                }
               

                var schedule = _dbContext.Schedules
                    .Where(s => s.AppUserId == masterId 
                        && (s.Date != null ? 
                            (s.Date > DateTime.UtcNow && s.Date < DateTime.UtcNow.AddDays(daysAhead)) : true))
                    .ToList();
                if (schedule != null)
                {
                    res.Data = _mapper.Map<List<ScheduleDto>>(schedule);
                }
                else
                {
                    res.ErrorMessage = "No schedule was found";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
            }
            return res;
        }

        public Result<bool> UpdateMasterSchedule(List<ScheduleDto> schedule)
        {
            var res = new Result<bool>();
            try
            {
                var scheduleToUpdate = _mapper.Map<List<Schedule>>(schedule);
                if (scheduleToUpdate != null)
                {
                    _dbContext.UpdateRange(scheduleToUpdate);
                    res.Data = _dbContext.SaveChanges() > 0;
                }
                else
                {
                    res.ErrorMessage = "No data to update";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
            }
            return res;
        }

        public async Task<Result<bool>> DeleteSchedule(int scheduleId)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var scheduleToDelete = _dbContext.Schedules.FirstOrDefault(s => s.Id == scheduleId && s.AppUserId != null);
                    if(scheduleToDelete != null)
                    {
                        var isWorker = IsInOrganization(user.Id, scheduleToDelete.AppUserId);
                        if (scheduleToDelete.AppUserId == user.Id || isWorker)
                        {
                            _dbContext.Remove(scheduleToDelete);
                            res.Data = _dbContext.SaveChanges() > 0;
                        }
                        else
                        {
                            res.ErrorMessage = "You don't have access to this action";
                        }
                    }
                    else
                    {
                        res.ErrorMessage = "THere is no such schedule";
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
            }

            return res;
        }

        public async Task<Result<bool>> AddMasterSchedule(ScheduleDto schedule)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations
                       .FirstOrDefault(x => x.AppUserId == user.Id);
                    var isWorker = IsInOrganization(user.Id, schedule.AppUserId ?? "");
                    if(organization == null || isWorker)
                    {
                        if(organization == null)
                        {
                            schedule.AppUserId = user.Id;
                        }
                        var scheduleToAdd = _mapper.Map<Schedule>(schedule);
                        _dbContext.Add(scheduleToAdd);
                        res.Data = _dbContext.SaveChanges() > 0;
                    }
                    else
                    {
                        res.ErrorMessage = "You don't have access to this action";
                    }
                }
                else
                {
                    res.ErrorMessage = "Error with schedule owner";
                }
            }
            catch(Exception ex) 
            { 
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        private bool IsInOrganization(string orgOwnerId, string masterId)
        {
            var organization = _dbContext.Organizations
                        .Include(o => o.OrganizationEmployees)
                        .FirstOrDefault(x => x.AppUserId == orgOwnerId);
            if(organization != null)
            {
                var res = organization != null ? organization.OrganizationEmployees
                            .Where(e => e.AppUserId == masterId && e.DismissalDate == null).Any() : false;
                if(orgOwnerId == masterId)
                {
                    return true;
                }
                return res;
            }

            return false;
        }
    }   

}
