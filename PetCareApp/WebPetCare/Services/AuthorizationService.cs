﻿using System.Security.Claims;
using PetCareApp.Dtos;
using System.Text.Json;
using Azure;
using System.Net.Http;
using Microsoft.JSInterop;
using System.Net;
using WebPetCare.IServices;
using PetCareApp.Models;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebPetCare.Services
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJSRuntime _jsRuntime;
        public AuthorizationService(HttpClient httpClient, IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor, IJSRuntime jsRuntime)
        : base(httpClient, configuration, jsRuntime)
        {
            _httpContextAccessor = httpContextAccessor;
            _jsRuntime = jsRuntime;
        }

        public async Task<ResultData> Login(LoginDto loginDto)
        {
            var res = new ResultData();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(loginDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = string.Empty;
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/Login";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(result, options);
                    if (user != null)
                    {
                        Logout();
                        await SetStoreItemAsync(user.Token, "token");
                        await SetStoreItemAsync(user.Role, "role");
                        await SetStoreItemAsync("local", "auth_method");

                    }
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        res.StatusCode = 403;
                        var emailRes = await SendEmail(loginDto.Email);
                        res.Result = emailRes.Result;
                        res.Error += $"\n{emailRes.Error}";

                    }
                    error = await response.Content.ReadAsStringAsync();
                }

                res.Error = error;
            }
            catch (Exception ex)
            {
                res.Error = ex.Message;
            }
            return res;
        }

        public async Task<ResultData> Register(RegisterDto registerDto, string role)
        {
            var result = new ResultData();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(registerDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/register?role={role}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseResult = await response.Content.ReadAsStringAsync();

                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(responseResult, options);
                    if (user != null)
                    {
                        await SetStoreItemAsync(user.Token, "token");
                        await SetStoreItemAsync(user.Role, "role");
                        await SetStoreItemAsync("local", "auth_method");

                        result = await SendEmail(user.Email);
                    }
                }
                else
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<ResultData> SendEmail(string email, bool IsPasswordRestore = false)
        {
            var result = new ResultData();
            var randomNum = GenerateCode();
            var data = new EmailConfirmationDto { Email = email, checkNumber = randomNum, IsPasswordChange = IsPasswordRestore };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(data, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/sendEmail";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    result.Result = randomNum;
                }
                else
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private string GenerateCode()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }

        public async Task<bool> ConfirmEmail(string email, string? newPassword)
        {
            var client = new HttpClient();
            try
            {
                var type = newPassword != null ? $"&newPassword={newPassword}" : "";
                string fullUrl = $"{_apiUrl}/api/account/confirmEmail?email={email}{type}";

                HttpResponseMessage response = await client.PatchAsync(fullUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task SetStoreItemAsync(string token, string name)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorageSetItem", name, token);
        }

        public async Task DeleteStoreItemAsync(string name)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorageRemoveItem", name);
        }

        public async Task<string> Logout()
        {
            try
            {
                await DeleteStoreItemAsync("token");
                await DeleteStoreItemAsync("role");
                await DeleteStoreItemAsync("auth_method");
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<ResultData> SendEmailForPasswordRestore(string email)
        {
            var res = new ResultData();
            if (String.IsNullOrEmpty(email))
            {
                res.Error = "Email field is empty";
            }
            else
            {
                var data = await SendEmail(email, true);
                return data;
            }

            return res;
        }


        public  async Task<Result<bool>> CheckGoogleLogin(string email, string id)
        {
            var res= new Result<bool>();
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/googleCheckEmail?email={email}&id={id}";

                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(result, options);
                    if (user != null)
                    {
                        await SetStoreItemAsync(user.Token, "token");
                        await SetStoreItemAsync(user.Role, "role");
                        await SetStoreItemAsync("google", "auth_method");
                        res.Data = true;
                    }
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        res.Data = false;

                    }
                    res.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<bool>> RegisterGoogleUser(AppUser userDto, string role)
        {

            var res = new Result<bool>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(userDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/addGoogleUser?role={role}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseResult = await response.Content.ReadAsStringAsync();

                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(responseResult, options);
                    if (user != null)
                    {
                        await SetStoreItemAsync(user.Token, "token");
                        await SetStoreItemAsync(user.Role, "role");
                        await SetStoreItemAsync("google", "auth_method");

                        res.Data = true;
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

    public class ResultData //remake to use result generic class
    {
        public string Result { get; set; }
        public string Error { get; set; }
        public int? StatusCode { get; set; }
    }
}
