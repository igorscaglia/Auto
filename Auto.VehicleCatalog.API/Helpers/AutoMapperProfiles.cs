using AutoMapper;
using Auto.VehicleCatalog.API.Dtos;
using Auto.VehicleCatalog.API.Model;

namespace Auto.VehicleCatalog.API.Helpers
{
    // This class is responsible for mapping your domain model to correspondent DTO and vice versa
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Config mapping from Brand to BrandForList
            CreateMap<Brand, BrandForList>()
                .ForMember(dest => dest.name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id));

            // Config mapping from Brand to BrandForDetail
            CreateMap<Brand, BrandForDetail>()
                .ForMember(dest => dest.name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id));

            // Config mapping from BrandForNew to Brand
            CreateMap<BrandForNew, Brand>()
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.name));


            // Config mapping from Model to ModelForList
            CreateMap<Model.Model, ModelForList>()
                .ForMember(dest => dest.name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id));

            // Config mapping from Model to ModelForDetail
            CreateMap<Model.Model, ModelForDetail>()
                .ForMember(dest => dest.name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.brandId,
                            opt => opt.MapFrom(src => src.BrandId));

            // Config mapping from ModelForNew to Model
            CreateMap<ModelForNew, Model.Model>()
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.BrandId,
                            opt => opt.MapFrom(src => src.brandId));


            // Config mapping from Vehicle to VehicleForList
            CreateMap<Vehicle, VehicleForList>()
                .ForMember(dest => dest.value,
                            opt => opt.MapFrom(src => src.Value.ToPtBrFormat()))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.brand,
                            opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.model,
                            opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.yearModel,
                            opt => opt.MapFrom(src => src.YearModel))
                .ForMember(dest => dest.fuel,
                            opt => opt.MapFrom(src => src.Fuel));

            // Config mapping from Vehicle to VehicleForDetail
            CreateMap<Vehicle, VehicleForDetail>()
                .ForMember(dest => dest.value,
                            opt => opt.MapFrom(src => src.Value.ToPtBrFormat()))
                .ForMember(dest => dest.id,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.brand,
                            opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.brandId,
                            opt => opt.MapFrom(src => src.BrandId))                            
                .ForMember(dest => dest.model,
                            opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.modelId,
                            opt => opt.MapFrom(src => src.ModelId))                            
                .ForMember(dest => dest.yearModel,
                            opt => opt.MapFrom(src => src.YearModel))
                .ForMember(dest => dest.fuel,
                            opt => opt.MapFrom(src => src.Fuel));

            // Config mapping from Vehicle to VehicleForNew
            CreateMap<VehicleForNew, Vehicle>()
                .ForMember(dest => dest.Value,
                            opt => opt.MapFrom(src => src.value))
                .ForMember(dest => dest.BrandId,
                            opt => opt.MapFrom(src => src.brandId))
                .ForMember(dest => dest.ModelId,
                            opt => opt.MapFrom(src => src.modelId))
                .ForMember(dest => dest.YearModel,
                            opt => opt.MapFrom(src => src.yearModel))
                .ForMember(dest => dest.Fuel,
                            opt => opt.MapFrom(src => src.fuel));

            // Config mapping from UserForRegister to User
            CreateMap<UserForRegister, User>()
                .ForMember(dest => dest.UserName,
                            opt => opt.MapFrom(src => src.username));
        }
    }
}
