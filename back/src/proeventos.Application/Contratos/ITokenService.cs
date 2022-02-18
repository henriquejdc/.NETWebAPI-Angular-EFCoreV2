using System.Threading.Tasks;
using proeventos.Application.Dtos;

namespace proeventos.Application.Contratos
{
    public interface ITokenService
    {
         Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}