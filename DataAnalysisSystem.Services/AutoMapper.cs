using AutoMapper;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.UserDTO;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterViewModel, IdentityProviderUser>()
                     .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email))
                     .ForMember(dest => dest.NormalizedUserName, opts => opts.MapFrom(src => src.Email.ToUpper()))
                     .ForMember(dest => dest.NormalizedEmail, opts => opts.MapFrom(src => src.Email.ToUpper()))
                     .ForMember(dest => dest.UserDatasets, opts => opts.MapFrom(src => new List<string>()))
                     .ForMember(dest => dest.UserAnalyses, opts => opts.MapFrom(src => new List<string>()))
                     .ForMember(dest => dest.SharedDatasetsToUser, opts => opts.MapFrom(src => new List<string>()))
                     .ForMember(dest => dest.SharedAnalysesToUser, opts => opts.MapFrom(src => new List<string>()));

            CreateMap<IdentityProviderUser, EditUserDataViewModel>()
                         .ForMember(dest => dest.UserIdentificator, opts => opts.MapFrom(src => src.Id));


            CreateMap<EditUserDataViewModel, IdentityProviderUser>()
                         .ForMember(dest => dest.Id, opts => opts.Ignore())
                         .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email))
                         .ForMember(dest => dest.NormalizedUserName, opts => opts.MapFrom(src => src.Email.ToUpper()))
                         .ForMember(dest => dest.NormalizedEmail, opts => opts.MapFrom(src => src.Email.ToUpper()));
        }
    }
}
