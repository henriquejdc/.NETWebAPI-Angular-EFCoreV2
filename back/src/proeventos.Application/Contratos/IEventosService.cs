using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Application.Contratos
{
    public interface IEventoService
    {
         Task<Evento> AddEventos(Evento model);
         Task<Evento> UpdateEventos(int eventoId, Evento model);
         Task<bool> DeleteEventos(int eventoId);
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);         
         Task<Evento[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes = false);
         Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);
    }
}