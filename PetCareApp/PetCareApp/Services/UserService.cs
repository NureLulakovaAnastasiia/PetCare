using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;
using System.Net.WebSockets;

namespace PetCareApp.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext) : base(userManager, httpContextAccessor)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
        }

        private void AddHistoryEvent(HistoryEvent historyEvent)
        {
            try
            {
                _dbContext.Add(historyEvent);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
        public async Task<Result<int>> AddPet(PetDto pet)
        {
            var res = new Result<int>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "User not found";
                    return res;
                }
                var petToAdd = _mapper.Map<Pet>(pet);
                if (petToAdd != null)
                {
                    petToAdd.AppUserId = user.Id;
                    _dbContext.Add(petToAdd);
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateNewPetEvent(user.Id, petToAdd.Name));
                    }
                    res.Data = petToAdd.Id;
                    return res;
                }
                res.ErrorMessage = "Error during creating";
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<string> UpdatePets(List<PetDto> pets)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var petsToUpdate = _mapper.Map<List<Pet>>(pets);
                if (petsToUpdate != null)
                {
                    foreach (var item in petsToUpdate)
                    {
                        item.AppUserId = user.Id;
                        _dbContext.Update(item);
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemovePet(int petId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var userPets = _dbContext.Pets.Where(x => x.AppUserId == user.Id);
                if (userPets != null)
                {
                    var item = userPets.First(x => x.Id == petId);
                    if (item != null && item.AppUserId == user.Id)
                    {
                        _dbContext.Pets.Remove(item);
                    }
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateDeletePetEvent(user.Id, item.Name));
                    }
                    return result.ToString();
                }
                return "Error during deleting";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public MasterDto GetMasterData(string masterId)
        {
            var res = new MasterDto();
            var master = _dbContext.Users
                .Where(u => u.Id == masterId)
                .Include(u => u.Contacts)
                .ThenInclude(c => c.Location)
                .Include(u => u.Schedules)
                .Include(u => u.Services)
                .Include(u => u.Reviews)
                .Include(u => u.Portfolios)
                .Include(u => u.OrganizationEmployee)
                .FirstOrDefault();

            if(master != null)
            {
                var services = master.Services.Select(s => s.Id).ToList();
                var reviews = services != null ? _dbContext.Reviews.Where(r => services.Contains(r.ServiceId ?? 0)).ToList() : null;
                res.Id = master.Id;
                res.FirstName = master.FirstName;
                res.LastName = master.LastName;
                res.Photo = master.Photo;
                res.Rate = reviews != null && reviews.Count > 0 ? reviews.Average(u => u.Rate) : 0;
                res.Schedules = _mapper.Map<List<ScheduleDto>>(master.Schedules);
                res.Contacts = ContactsMapping.MapContact(master.Contacts);
                res.Portfolios = _mapper.Map<List<PortfolioDto>>(master.Portfolios);
                res.Schedules = res.Schedules.Where(s => s.Date == null || s.Date > DateTime.Now).ToList();
                if (master.OrganizationEmployee != null && master.OrganizationEmployee.Count > 0)
                {
                    var currentOrg = master.OrganizationEmployee.FirstOrDefault(e => e.DismissalDate == null);
                    if (currentOrg != null)
                    {
                        var org = _dbContext.Organizations.FirstOrDefault(o => o.Id == currentOrg.OrganizationId);
                        if (org != null)
                        {
                            res.OrgName = org.Name;
                            res.OrgId = org.Id;
                        }
                    }
                }
            }

            return res;
        }

        public async Task<Result<List<ReviewDto>>> GetMasterReviews(string masterId)
        {
           
            var res = new Result<List<ReviewDto>>();
            var user = await GetCurrentUserAsync();
            if (String.IsNullOrEmpty(masterId))
            {
                if (user != null)
                {
                    masterId = user.Id;
                }
                else
                {
                    return res;
                }
            }
            var data = _dbContext.Reviews
                .Include(r => r.AppUser)
                .Include(r => r.Service)
                .Include(r => r.Comments)
                .ThenInclude(c => c.AppUser)
                .Where(r => r.Service.AppUserId == masterId)
                .ToList();
            if (data.Count > 0)
            {
                res.Data = new List<ReviewDto>();
                foreach (var item in data)
                {
                    var mapped = ReviewMapping.MapReview(item);
                    if (user != null)
                    {

                        if (mapped.AppUserId == user.Id)
                        {
                            mapped.isYours = true;
                        }
                        foreach (var comment in mapped.Comments)
                        {
                            if (comment.AppUserId == user.Id)
                            {
                                comment.isYours = true;
                            }
                        }
                    }
                    res.Data.Add(mapped);

                }
            }

            return res;
        }

        public async Task<List<RecordInfoDto>> GetUserRecords()
        {
            var res = new List<RecordInfoDto>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return res;
                }
                var records = _dbContext.Records.Where(r => r.AppUserId == user.Id)
                    .Include(r => r.Service)
                    .Include(r => r.RecordCancel)
                    .Include(r => r.Pet)
                    .ToList();
                var services = _dbContext.Services.Where(s => s.AppUserId == user.Id).Select(s => s.Id).ToList();
                var servicesWithReviews = _dbContext.Reviews.Where(r => r.AppUserId == user.Id).Select(r => r.ServiceId).Distinct().ToList();
                if (services.Count > 0)
                {
                    records = records.Where(r => !services.Contains(r.ServiceId ?? 0)).ToList();
                }
                if (records != null && records.Count > 0)
                {
                    foreach(var record in records)
                    {
                        var mapped = RecordMapping.MapRecordToInfo(record);
                        if (servicesWithReviews.Contains(mapped.ServiceId))
                        {
                            mapped.IsReviewed = true;
                        }
                        res.Add(mapped);
                    } 
                }
            }
            catch (Exception ex)
            {
                
            }

            return res;
        }

        public async Task<Result<bool>> CancelRecord(int recordId)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "There is no such user";
                    return res;
                }
                var record = _dbContext.Records.Where(r => r.Id == recordId)
                    .Include(r => r.Service).FirstOrDefault();
                if (record != null)
                {
                    if(record.AppUserId == user.Id)
                    {
                        var cancel = new RecordCancel
                        {
                            AppUserId = user.Id,
                            RecordId = recordId,
                            Reason = "Cancelled by user",
                            Date = DateTime.UtcNow
                        };
                        record.Status = "Cancelled";
                        _dbContext.Add(cancel);
                        _dbContext.Update(record);
                        var result = _dbContext.SaveChanges();
                        if (result > 0)
                        {
                            AddHistoryEvent(HistoryEventFactory.CreateRecordCancellationEvent(user.Id, record.Service.Name, record.StartTime));
                            AddHistoryEvent(HistoryEventFactory.CreateRecordCancellationEvent(record.Service.AppUserId, record.Service.Name, record.StartTime));

                        }
                        res.Data = result > 1;

                    }
                    else
                    {
                        res.ErrorMessage = "You don't have an access to this record";
                    }
                }
                else
                {
                    res.ErrorMessage = "Record was not found";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.ToString();
            }

            return res;
        }

        public async Task<Result<bool>> AddReview(ReviewDto review)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "There is no such user";
                    return res;
                }
                var service = _dbContext.Services.FirstOrDefault(r => r.Id == review.ServiceId);
                if (service != null)
                {
                    var reviewToAdd = _mapper.Map<Review>(review);
                    if (reviewToAdd != null)
                    {
                        reviewToAdd.AppUserId = user.Id;
                        _dbContext.Add(reviewToAdd);
                        var result = _dbContext.SaveChanges();
                        if (result > 0)
                        {
                            AddHistoryEvent(HistoryEventFactory.CreateNewReviewEvent(review.AppUserId, service.Name));
                            AddHistoryEvent(HistoryEventFactory.CreateNewReviewEventMaster(service.AppUserId, service.Name));

                        }
                        res.Data = result == 1;
                    }
                    else
                    {
                        res.ErrorMessage = "Error during mapping";
                    }
                }
                else
                {
                    res.ErrorMessage = "Service was not found";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.ToString();
            }

            return res;
        }

        public async Task<Result<bool>> RemoveReview(int reviewId)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "There is no such user";
                    return res;
                }

                var review = _dbContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
                if (review != null && review.AppUserId == user.Id)
                {
                    var comments = _dbContext.ReviewComments.Where(r => r.ReviewId == reviewId).ToList();
                    if (comments != null && comments.Any())
                    {
                        _dbContext.RemoveRange(comments);
                    }
                    _dbContext.Remove(review);
                    res.Data = _dbContext.SaveChanges() > 0;
                }
                else
                {
                    res.ErrorMessage = "No such review";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<bool>> RemoveReviewComment(int commentId)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "There is no such user";
                    return res;
                }

                var comment = _dbContext.ReviewComments.FirstOrDefault(c => c.Id == commentId);
                if (comment != null && comment.AppUserId == user.Id)
                {
                    _dbContext.Remove(comment);
                    res.Data = _dbContext.SaveChanges() > 0;
                }
                else
                {
                    res.ErrorMessage = "No such comment";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<bool>> AddReviewComment(ReviewCommentDto reviewComment)
        {
            var res = new Result<bool>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "There is no such user";
                    return res;
                }
                var review = _dbContext.Reviews.Where(r => r.Id == reviewComment.ReviewId)
                    .Include(r => r.Service).FirstOrDefault();
                if (review != null)
                {
                    var comment = _mapper.Map<ReviewComment>(reviewComment);
                    if (comment != null)
                    {
                        comment.AppUserId = user.Id;
                        _dbContext.Add(comment);
                        var result = _dbContext.SaveChanges();
                        if (result > 0)
                        {
                            AddHistoryEvent(HistoryEventFactory.CreateNewReviewCommentEvent(review.AppUserId, review.Service.Name));
                        }

                        res.Data = result > 0;


                    }
                }
                else
                {
                    res.ErrorMessage = "No such review";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<List<HistoryEvent>>> GetEventsHistory()
        {
           var res = new Result<List<HistoryEvent>>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "Not authorized";
                }
                else
                {
                    var events = _dbContext.HistoryEvent
                        .Where(e => e.AppUserId == user.Id)
                        .OrderByDescending(e => e.Created)
                        .ToList();
                    if(events != null)
                    {
                        events.ForEach(e => e.AppUser = null);
                        res.Data = events;
                    }
                    else
                    {
                        res.ErrorMessage = "No data found";
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }
    }
}
