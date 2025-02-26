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
        } //for master

        public static HistoryEvent CreateNewOrgRequestOrgEvent(string appUserId, string masterName) //for organization
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Organization request",
                Description = $"You have new request from {masterName}. "
            };
        }

        public static HistoryEvent CreatePasswordChangeEvent(string appUserId, bool isSuccess)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Password change",
                Description = isSuccess ? "Password was changed succesfully." : "Error occured while changing password. Try again."
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

        public static HistoryEvent CreateAcceptRequestEvent(string appUserId, string masterName) //for organization
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Accepted Request",
                Description = $"You succesfully accepted request from {masterName}. Now he/she became your employee."
            };
        }

        public static HistoryEvent CreateAcceptRequestMasterEvent(string appUserId, string orgName) //for master
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Accepted Request",
                Description = $"Your request to {orgName} was succesfully accepted. Now you became its employee."
            };
        }

        public static HistoryEvent CreateRejectRequestEvent(string appUserId, string masterName) //for organization
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Rejected Request",
                Description = $"You rejected request from {masterName}."
            };
        }

        public static HistoryEvent CreateRejectRequestMasterEvent(string appUserId, string orgName) //for master
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Rejected Request",
                Description = $"Your request to {orgName} was rejected."
            };
        }

        public static HistoryEvent CreateOrgScheduleUpdateEvent(string appUserId) //for master
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Updated schedule",
                Description = $"Your schedule was updated by your organization."
            };
        }

        public static HistoryEvent CreateDismissEmployeeEvent(string appUserId, string orgName) //for master
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Employee dismiss",
                Description = $"You were dismissed from your organization {orgName}."
            };
        }

        public static HistoryEvent CreateDismissEmployeeOrgEvent(string appUserId, string masterName) //for organization
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Employee dismiss",
                Description = $"Yu have dismissed master {masterName} from your organization."
            };
        }

        public static HistoryEvent CreateOrgAddPortfolioEvent(string appUserId, string orgName) //for master
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Portfolio addition",
                Description = $"Your potfolio were added to your organization {orgName} portfolio."
            };
        }

        public static HistoryEvent CreateOrgAddPortfolioOrgEvent(string appUserId, string masterName) //for organization
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Portfolio addition",
                Description = $"You have added {masterName} potfolio to your organization portfolio."
            };
        }

        public static HistoryEvent CreateNewServiceEvent(string appUserId, string serviceName) 
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "New service",
                Description = $"You have succesfully created new service \"{serviceName}\"."
            };
        }

        public static HistoryEvent CreateDeleteServiceEvent(string appUserId, string serviceName)
        {
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Service deletion",
                Description = $"You have succesfully deleted your service \"{serviceName}\"."
            };
        }

        public static HistoryEvent CreateChangeServiceVisibilityEvent(string appUserId, string serviceName, bool isHidden)
        {
            var type = isHidden ? "hidden" : "visible";
            return new HistoryEvent
            {
                AppUserId = appUserId,
                Title = "Service visibility",
                Description = $"You made your service \"{serviceName}\"  { type } to public."
            };
        }

    }

}
