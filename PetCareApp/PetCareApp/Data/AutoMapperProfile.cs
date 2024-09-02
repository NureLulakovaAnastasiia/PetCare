using PetCareApp.Dtos;
using PetCareApp.Models;
using AutoMapper;

namespace PetCareApp.Data
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() {

            CreateMap<AddContactsDto, Contacts>();
            CreateMap<ContactsDto, Contacts>();
            CreateMap<AddServiceDto, Models.Service>();
        }
        
    }
}
