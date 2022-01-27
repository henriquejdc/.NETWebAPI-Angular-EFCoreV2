using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface IEventoPersistence
    {
         //Eventos
         Task<Evento[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes = false);
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
         Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);

    }
}