using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings
{
    public class EntitiesToDTOPaggingProfile : Profile
    {
        public EntitiesToDTOPaggingProfile()
        {
            CreateMap<Seguro, SeguroDTO>().ReverseMap();
        }
    }
}
