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
            CreateMap<RequestToOrganization, GetRequestDto>()
                .ForMember(dest => dest.UserName, opt => opt
                .MapFrom(src => src.AppUser != null ?(src.AppUser.LastName + " " + src.AppUser.FirstName) : ""))
                .ForMember(dest => dest.OrganizationName, opt => opt
                .MapFrom(src => src.Organization != null ? src.Organization.Name : ""));
            CreateMap<AddAnswerDto, Answer>();
            CreateMap<AddQuestionDto, Question>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));
            CreateMap<ScheduleDto, Schedule>();
            CreateMap<BreakDto, Break>();
            CreateMap<Break, BreakDto>();
            CreateMap<RecordDto, Record>();
            CreateMap<Record, GetRecordDto>();
            CreateMap<AddPortfolioDto, Portfolio>();
            CreateMap<PortfolioDto, Portfolio>();
            CreateMap<Portfolio, PortfolioDto>();
            CreateMap<Contacts, GetContactsDto>();
            CreateMap<Contacts, ContactsDto>();
            CreateMap<GetContactsDto, Contacts>();
            CreateMap<Models.Service, GetServiceDto>();
            CreateMap<Pet,  PetDto>();
            CreateMap<PetDto, Pet>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<ScheduleDto, Schedule>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Organization, OrganizationDetailsDto>();
            CreateMap<ContactsDto, GetContactsDto>();
            CreateMap<ContactsDto, GetContactsDto>();
            CreateMap<OrganizationEmployee, GetEmployeeDto>()
                .ForMember(dest => dest.Name, opt => opt
                .MapFrom(src => src.AppUser != null ? (src.AppUser.LastName + " " + src.AppUser.FirstName) : ""))
                .ForMember(dest => dest.masterId, opt => opt
                .MapFrom(src => src.AppUser != null ? src.AppUser.Id : ""))
                .ForMember(dest => dest.OrganizationName, opt => opt
                .MapFrom(src => src.Organization != null ? src.Organization.Name : ""));
            CreateMap<Portfolio, OrganizationPortfolioDto>()
                .ForMember(dest => dest.masterName, opt => opt
                .MapFrom(src => src.AppUser != null ? (src.AppUser.LastName + " " + src.AppUser.FirstName) : ""))
;
        }

    }
}
