using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface IRedeSocialPersistence : IGeralPersistence
    {
         Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id);
         Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);
         Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
         Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}