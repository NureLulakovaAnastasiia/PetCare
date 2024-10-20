using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using PetCareApp.Dtos;
using System.Net;
using System.Text.Json;
using WebPetCare.IServices;
using PetCareApp.Models;

namespace WebPetCare.Services
{
    public class UserService : BaseService, IUserService
    {
        private  HttpClient httpClient { get; set; }
        private IJSRuntime jsRuntime;
        public UserService(HttpClient httpClientbase, IConfiguration configuration, IJSRuntime runtime) :
            base(httpClientbase, configuration, runtime) 
        {
            httpClient = httpClientbase;
            jsRuntime = runtime;
        }
        public async Task<Result<GetGeneralMasterDto>> getGeneralMasterData()
        {
            var res = new Result<GetGeneralMasterDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getMasterGeneralData";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<GetGeneralMasterDto>(result, options);
                    if (data != null)
                    {
                        res.Data = data;
                    }
                }
                else
                {
                    res.ErrorMessage = await response.Content.ReadAsStringAsync();
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
