﻿using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetGeneralMasterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[]? Photo { get; set; }

        public ContactsDto Contacts { get; set; } = new ContactsDto();
        public List<PetDto> Pets { get; set; } = new List<PetDto>();
        public List<GetServiceDto> Services { get; set; } = new List<GetServiceDto>();

        public GetGeneralMasterDto() { }
        public GetGeneralMasterDto(GetGeneralMasterDto dto) { 
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.Email = dto.Email;
            this.Photo = dto.Photo;
        }
    }
}
