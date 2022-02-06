using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface ILotePersistence
    {
         //Lotes
         Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
         Task<Lote> GetLoteByIdsAsync(int eventoId, int Id);

    }
}