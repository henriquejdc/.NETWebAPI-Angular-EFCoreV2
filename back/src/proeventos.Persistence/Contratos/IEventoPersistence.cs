using System.Threading.Tasks;
using proeventos.Domain;
using proeventos.Persistence.Models;

namespace proeventos.Persistence.Contratos
{
    public interface IEventoPersistence
    {
         //Eventos
         Task<PageList<Evento>> GetAllEventosAsync(int userId, 
                                                   PageParams pageParams, 
                                                   bool includePalestrantes = false);
         Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);

    }
}