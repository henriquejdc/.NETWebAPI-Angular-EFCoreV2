using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;

namespace proeventos.Persistence
{
    public class palestrantePersistence : IPalestrantePersistence
    {
        private readonly proeventosContext _context;
        public palestrantePersistence(proeventosContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(e => e.RedesSociais);

            if (includeEventos){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string Nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(e => e.RedesSociais);

            if (includeEventos){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id).Where(p => p.Nome.ToLower()
            .Contains(Nome.ToLower()));;

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(e => e.RedesSociais);

            if (includeEventos){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

    }
}