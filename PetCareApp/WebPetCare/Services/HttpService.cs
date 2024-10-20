using Azure.Core;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebPetCare.Services
{
    public static class HttpService
    {
        
        public static async Task<HttpClient> GetHttpClient(HttpClient client, IJSRuntime jSRuntime)
        {
            var token = await jSRuntime.InvokeAsync<string>("sessionStorageGetItem", "token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization
                           = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
    }
}
