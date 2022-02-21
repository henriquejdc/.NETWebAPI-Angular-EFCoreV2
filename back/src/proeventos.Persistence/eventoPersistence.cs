using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;
using proeventos.Persistence.Models;

namespace proeventos.Persistence
{
    public class EventoPersistence : IEventoPersistence
    {
        private readonly proeventosContext _context;
        public EventoPersistence(proeventosContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }
        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, 
                                                               PageParams pageParams,  
                                                               bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(e => (e.Tema.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      e.Local.ToLower().Contains(pageParams.Term.ToLower())) &&  
                                e.UserId == userId);

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.Where(e => e.Id == EventoId &&
                                e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

    }
}