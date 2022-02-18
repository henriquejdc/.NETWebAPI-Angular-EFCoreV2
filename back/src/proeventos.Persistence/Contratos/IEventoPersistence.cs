using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface IEventoPersistence
    {
         //Eventos
         Task<Evento[]> GetAllEventosByTemaAsync(int userId, string Tema, bool includePalestrantes = false);
         Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
         Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);

    }
}