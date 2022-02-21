using System.Threading.Tasks;
using proeventos.Application.Dtos;
using proeventos.Domain;
using proeventos.Persistence.Models;

namespace proeventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<EventoDto> AddEventos(int userId, EventoDto model);
         Task<EventoDto> UpdateEventos(int userId, int eventoId, EventoDto model);
         Task<bool> DeleteEventos(int userId, int eventoId);
         Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);         
         Task<EventoDto> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);
    }
}