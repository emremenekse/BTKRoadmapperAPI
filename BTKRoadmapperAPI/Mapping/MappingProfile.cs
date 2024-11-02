using AutoMapper;
using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<Module, ModuleDTO>().ReverseMap();
        }
    }
}
