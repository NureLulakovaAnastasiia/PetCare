using Azure.Core;
using Microsoft.JSInterop;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebPetCare.Services
{
    public static class HttpService
    {
        
        public static async Task<HttpClient> GetHttpClient(HttpClient client, IJSRuntime jSRuntime)
        {
            try
            {
                var token = await jSRuntime.InvokeAsync<string>("sessionStorageGetItem", "token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization
                               = new AuthenticationHeaderValue("Bearer", token);
                }
            }catch(Exception ex)
            {

            }
            return client;
        }

        public static async Task SetTokenToHttpClient(HttpClient client, IJSRuntime jsRuntime)
        {
            try
            {
                var token = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token");
                if (!string.IsNullOrEmpty(token))
                {
                    if (!client.DefaultRequestHeaders.Contains("Authorization"))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    }
                }
            }
            catch (JSDisconnectedException)
            {
                // JSRuntime is not available; skip setting header
            }
        }
    }
}
