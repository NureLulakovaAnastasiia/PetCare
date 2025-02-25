using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class HistoryEvent
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;

        public string? AppUserId { get; set; }

        public AppUser? AppUser { get; set; }

    }

    public static class HistoryEventFactory
    {
        public static HistoryEvent CreateQuestionaryUpdateEvent(string appUserId, string serviceName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Questionary update",
                Description = $"Questionary to service {serviceName} was succesfully updated."
            };
        }
        public static HistoryEvent CreateQuestionaryDeleteEvent(string appUserId, string serviceName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Questionary delete",
                Description = $"Questionary to service {serviceName} was succesfully deleted."
            };
        }

        public static HistoryEvent CreateNewAppointmentEvent(string appUserId, string serviceName, DateTime date)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New appointment",
                Description = $"Appointment to service {serviceName} at {date.ToString("F")}  was succesfully created."
            };
        }

        public static HistoryEvent CreateNewOrgRequestEvent(string appUserId, string orgName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Organization request",
                Description = $"Request to organization {orgName} was succesfully sent. "
            };
        }


        public static HistoryEvent CreatePasswordChangeEvent(string appUserId, bool isSuccess)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Password change",
                Description = isSuccess ? "Password was changed succesfully" : "Error occured while changing password. Try again."
            };
        }

        public static HistoryEvent CreateRecordCancellationEvent(string appUserId, string serviceName, DateTime recordDate)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Record cancellation",
                Description = $"Record for service \"{serviceName}\" at {recordDate.ToString("f")} was cancelled.",
                Created = DateTime.Now
            };
        }

        public static HistoryEvent CreateNewPetEvent(string appUserId, string PetName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New pet",
                Description = $"Your new pet {PetName} was succesfully added."
            };
        }

        public static HistoryEvent CreateDeletePetEvent(string appUserId, string PetName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Delete pet",
                Description = $"Your  pet {PetName} was succesfully deleted."
            };
        }

        public static HistoryEvent CreateNewReviewEvent(string appUserId, string serviceName) //for review owner
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New review",
                Description = $"Your review to service \"{serviceName}\" was succesfully added."
            };
        }

        public static HistoryEvent CreateNewReviewEventMaster(string appUserId, string serviceName) //for service owner
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New review",
                Description = $"You have new review to your service \"{serviceName}\"."
            };
        }

        public static HistoryEvent CreateNewReviewCommentEvent(string appUserId, string serviceName) 
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New review comment",
                Description = $"Your review to service \"{serviceName}\" got a comment from service provider."
            };
        }

    }

}
