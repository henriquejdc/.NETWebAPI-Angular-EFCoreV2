using AutoMapper;
using proeventos.Application.Dtos;
using proeventos.Domain;
using proeventos.Domain.Identity;
using proeventos.Persistence.Models;

namespace proeventos.Application.helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();

            CreateMap<Lote, LoteDto>().ReverseMap();

            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();

            CreateMap<Palestrante, PalestranteDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            
            CreateMap<User, UserLoginDto>().ReverseMap();
            
            CreateMap<User, UserUpdateDto>().ReverseMap();
            
        }
    }
}