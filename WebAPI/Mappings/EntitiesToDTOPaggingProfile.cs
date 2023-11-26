using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebApi.Mappings
{
    public class EntitiesToDTOPaggingProfile : Profile
    {
        public EntitiesToDTOPaggingProfile()
        {
            CreateMap<Seguro, SeguroDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
