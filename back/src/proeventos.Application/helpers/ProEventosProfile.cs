using AutoMapper;
using proeventos.Application.Dtos;
using proeventos.Domain;

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
            
        }
    }
}