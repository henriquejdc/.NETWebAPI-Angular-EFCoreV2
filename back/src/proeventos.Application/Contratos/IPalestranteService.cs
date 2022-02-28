using System.Threading.Tasks;
using proeventos.Application.Dtos;
using proeventos.Domain;
using proeventos.Persistence.Models;

namespace proeventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model);
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams,
                                                               bool includeEventos = false);         
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}