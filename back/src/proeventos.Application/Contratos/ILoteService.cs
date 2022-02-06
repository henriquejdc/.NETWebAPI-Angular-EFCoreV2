using System.Threading.Tasks;
using proeventos.Application.Dtos;
using proeventos.Domain;

namespace proeventos.Application.Contratos
{
    public interface ILoteService
    {
         Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] model);
         Task<bool> DeleteLote(int eventoId, int loteId);
         Task<LoteDto[]> GetLotesByEventoIdAsync(int loteId);         
         Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}