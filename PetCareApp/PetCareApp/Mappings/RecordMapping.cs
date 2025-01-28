using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Mappings
{
    public class RecordMapping
    {
        public static RecordInfoDto MapRecordToInfo(Record record)
        {
            var recordInfo = new RecordInfoDto()
            {
                Id = record.Id,
                AppUserId = record.AppUserId,
                ServiceId = record.ServiceId,
                Description = record.Description,
                StartTime = record.StartTime,
                EndTime = record.EndTime,
                Comment = record.Comment,
                Status = record.Status,
                RecordCancelReason = record.RecordCancel != null ? record.RecordCancel.Reason : string.Empty,
                ServiceName = record.Service != null ? record.Service.Name : string.Empty,
                PetId = record.PetId,
                PetName = record.Pet != null ? record.Pet.Name : string.Empty
            };
            return recordInfo;
        }
    }
}
