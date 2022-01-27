using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;

namespace proeventos.Persistence
{
    public class eventoPersistence : IEventoPersistence
    {
        private readonly proeventosContext _context;
        public eventoPersistence(proeventosContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            // exemplo para tirar o não tracking global
            // query = query.AsNoTracking().OrderBy(e => e.Id)
            
            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string Tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id).Where(e => e.Tema.ToLower()
            .Contains(Tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
            }

            query = query.Where(e => e.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }

    }
}