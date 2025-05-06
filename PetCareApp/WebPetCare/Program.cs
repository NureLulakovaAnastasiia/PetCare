using Microsoft.JSInterop;
using PetCareApp.Models;
using WebPetCare.Components;
using WebPetCare.IServices;
using WebPetCare.Services;
using Syncfusion;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using WebPetCare.Resources;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;



var builder = WebApplication.CreateBuilder(args);
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    options.ClaimActions.MapJsonKey("urn:google:given_name", "given_name", "string");
    options.ClaimActions.MapJsonKey("urn:google:family_name", "family_name", "string");
    options.Scope.Add("https://www.googleapis.com/auth/calendar.events");
    options.SaveTokens = true;
});
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpClient();
builder.Services.AddLocalization();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<GoogleCalendarService>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();

string[] supportedCultures =["en","uk"];


var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseCookiePolicy();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapGet("/login-google", async (HttpContext context) =>
{
   
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = $"/signin"
    });
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync("Cookies");

    var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";

    string googleLogoutUrl = $"https://accounts.google.com/Logout?continue=https://appengine.google.com/_ah/logout?continue={baseUrl}/";

    // Redirect user to Google logout URL
    context.Response.Redirect(googleLogoutUrl);


});

app.UseSession();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
