﻿using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IAccountService
    {
        string SendEmail(EmailConfirmationDto emailConfirmationDto);

        Task<List<string>> GetUserRoleByEmail(string email);

        string SubmitEmail(string email);

        public Task<string?> GetCurrentRole();

        public string CreateOrganization(string userId);

        public void AddChangePasswordHistory(string userId, bool isSuccess);

        //public Task<string> ChangeRoleToMaster();
    }
}
