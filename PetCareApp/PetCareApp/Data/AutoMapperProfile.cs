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
            CreateMap<AddAnswerDto, Answer>();
            CreateMap<AddQuestionDto, Question>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
            
        }
        
    }
}
