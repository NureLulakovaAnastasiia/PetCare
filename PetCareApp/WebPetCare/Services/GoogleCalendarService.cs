using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Syncfusion.Blazor.Calendars;
using PetCareApp.Models;
using Microsoft.Extensions.Logging;

namespace WebPetCare.Services
{
    public class GoogleCalendarService
    {
        private string? _accessToken;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string calendarId = "primary";
        private string AppName = "PetCareWeb";
        public GoogleCalendarService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string?> GetAccessTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            var result = await context.AuthenticateAsync("Cookies");
            if (!result.Succeeded)
            {
                return null;
            }

            return result.Properties?.GetTokenValue("access_token");
        }

        public async Task<bool> AddEventsAsync(List<Event> events)
        {
            _accessToken = await GetAccessTokenAsync();
            if (_accessToken != null)
            {
                try
                {
                    var credential = GoogleCredential.FromAccessToken(_accessToken);
                    var service = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = AppName
                    });
                    var date = events[0].Start.DateTime.Value;
                    
                    await ClearMonthEvents(service, date.Month, date.Year);

                    foreach (var newEvent in events)
                    {
                        newEvent.ExtendedProperties = new Event.ExtendedPropertiesData
                        {
                            
                            Private__ = new Dictionary<string, string>{
                                { "createdByApp", AppName }, 
                            }
                        };
                        var res = await service.Events.Insert(newEvent, calendarId).ExecuteAsync();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка додавання подій: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteEventsAsync(DateTime date)
        {
            _accessToken = await GetAccessTokenAsync();
            if (_accessToken != null)
            {
                try
                {
                    var credential = GoogleCredential.FromAccessToken(_accessToken);
                    var service = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = AppName
                    });
                    

                    await ClearMonthEvents(service, date.Month, date.Year);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;

        }


        private async Task<bool> ClearMonthEvents(CalendarService service, int month, int year)
        {
            try
            {
                var allEventsRequest = service.Events.List(calendarId);
                allEventsRequest.TimeMaxDateTimeOffset = new DateTimeOffset(new DateTime(year, month == 12 ? 1 : month + 1, 1));
                allEventsRequest.TimeMinDateTimeOffset = new DateTimeOffset(new DateTime(year, month, 1));
                allEventsRequest.PrivateExtendedProperty = $"createdByApp={AppName}";

                var allEvents = await allEventsRequest.ExecuteAsync();
                if (allEvents != null)
                {
                    if(allEvents.Items.Count > 0)
                    {
                        foreach (var item in allEvents.Items)
                        {
                            await service.Events.Delete(calendarId, item.Id).ExecuteAsync();
                        }
                    }
                    return true;
                }

                return false;

            } catch (Exception ex)
            {
                return false;
            }
        }


    }
}
