using System.Threading.Tasks;
using proeventos.Application.Dtos;
using proeventos.Domain;

namespace proeventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEventos(EventoDto model);
         Task<EventoDto> UpdateEventos(int eventoId, EventoDto model);
         Task<bool> DeleteEventos(int eventoId);
         Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);         
         Task<EventoDto[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes = false);
         Task<EventoDto> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);
    }
}