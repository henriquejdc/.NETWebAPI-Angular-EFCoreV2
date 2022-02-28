using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;
using proeventos.Persistence.Models;

namespace proeventos.Persistence
{
    public class PalestrantePersistence : GeralPersistence, IPalestrantePersistence
    {
        private readonly proeventosContext _context;
        public PalestrantePersistence(proeventosContext context) : base(context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(e => e.RedesSociais);

            if (includeEventos){
                query = query.Include(e => e.PalestranteEventos)
                             .ThenInclude(pe => pe.Evento);
            }

            query = query.Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) || 
                                      p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                      p.User.Funcao == Domain.Enums.Funcao.Palestrante);;
            query = query.AsNoTracking().OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(e => e.RedesSociais);

            if (includeEventos){
                query = query.Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

    }
}