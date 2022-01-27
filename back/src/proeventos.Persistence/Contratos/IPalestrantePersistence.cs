using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface IPalestrantePersistence
    {
         //Palestrantes
         Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string Nome, bool includeEventos = false);
         Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);
         Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false);

    }
}