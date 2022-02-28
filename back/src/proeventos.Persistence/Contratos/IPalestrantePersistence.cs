using System.Threading.Tasks;
using proeventos.Domain;
using proeventos.Persistence.Models;

namespace proeventos.Persistence.Contratos
{
    public interface IPalestrantePersistence : IGeralPersistence
    {
         //Palestrantes
         Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
         Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

    }
}