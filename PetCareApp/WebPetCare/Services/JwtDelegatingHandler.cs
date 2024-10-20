using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using WebPetCare.IServices;

namespace WebPetCare.Services
{
    public class JwtDelegatingHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public JwtDelegatingHandler(IJSRuntime jSRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jSRuntime;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_navigationManager.Uri.Contains("localhost") && _navigationManager.Uri.Contains("http"))
            {
                // Prerendering - skip token addition
                return await base.SendAsync(request, cancellationToken);
            }
            var token = await _jsRuntime.InvokeAsync<string>("sessionStorageGetItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
