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
            CreateMap<UpdateServiceDto, Models.Service>();
            CreateMap<Models.Service, GetServiceDto>();
            CreateMap<GetRequestDto, RequestToOrganization>();
            CreateMap<AddAnswerDto, Answer>();
            CreateMap<AddQuestionDto, Question>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
            CreateMap<ScheduleDto, Schedule>();
            CreateMap<BreakDto, Break>();
            CreateMap<RecordDto, Record>();
            CreateMap<AddPortfolioDto, Portfolio>();
            CreateMap<PortfolioDto, Portfolio>();
            CreateMap<Portfolio, PortfolioDto>();
            CreateMap<Contacts, GetContactsDto>();
            CreateMap<GetContactsDto, Contacts>();
            CreateMap<Models.Service, GetServiceDto>();
        }
        
    }
}
