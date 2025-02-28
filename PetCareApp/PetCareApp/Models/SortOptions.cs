using System;

namespace PetCareApp.Models
{
    public static class SortOptions
    {
        public enum SortDirection
        {
            Newest, Oldest
        }

        public enum SortTypesReview
        {
            Newest, Oldest, HighRate, LowRate
        }
        public enum SortTypesService
        {
            HighPrice, LowPrice, HighRate, LowRate
        }
        public enum FilterByRecordStatus
        {
            Created, Finished, Cancelled, All
        }

        public enum FilterByServiceVisibility
        {
            Hidden, Visible, All
        }

        public enum FilterByRequestStatus
        {
            New, Rejected, Accepted, All
        }

        public enum FilterEmployees
        {
            Working, Fired, All
        }

    }

    public class SelectListItem<TEnum> where TEnum : Enum
    {
        public string Text { get; set; }
        public TEnum Value { get; set; }

        public SelectListItem(TEnum value)
        {
            Value = value;
            Text = value.ToString(); 
        }

        public SelectListItem(TEnum value, string text = null)
        {
            Value = value;
            Text = text ?? value.ToString();
        }
    }
}

