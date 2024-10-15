using WebPetCare.Components.IServices;

namespace WebPetCare.Services
{
    public class JwtMiddleware
    {
        private readonly ITokenService _tokenService;

        public JwtMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = _tokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }

            await next(context);
        }
    }
}
