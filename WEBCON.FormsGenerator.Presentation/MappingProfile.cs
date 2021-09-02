using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;
using WEBCON.FormsGenerator.Presentation.ViewModels;

namespace WEBCON.FormsGenerator.Presentation
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Form, FormViewModel>();
            CreateMap<FormViewModel, Form>();
            CreateMap<FormContentField, FormContentFieldViewModel>()
                .ForMember(d => d.BpsFormFieldGuid, s => s.MapFrom(src => src.BpsFormField.Guid))
                .ForMember(d => d.BpsName, s => s.MapFrom(src => src.BpsFormField.Name))
                .ForMember(d => d.Type, s => s.MapFrom(src => src.BpsFormField.Type))
                .ForMember(d => d.BpsIsReadonly, s => s.MapFrom(src => src.BpsFormField.IsReadonly))
                .ForMember(d => d.BpsIsRequired, s => s.MapFrom(src => src.BpsFormField.IsRequired));
            CreateMap<FormContentFieldViewModel, FormContentField>();
            CreateMap<FormContentViewModel, FormContent>();
            CreateMap<FormContent, FormContentViewModel>();
        }
    }
}
