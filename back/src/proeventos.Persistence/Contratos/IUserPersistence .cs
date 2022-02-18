using System.Collections.Generic;
using System.Threading.Tasks;
using proeventos.Domain.Identity;

namespace proeventos.Persistence.Contratos
{
    public interface IUserPersistence : IGeralPersistence
    {
         Task<IEnumerable<User>> GerUsersAsync();
         Task<User> GerUserByIdAsync(int id);
         Task<User> GetUserByUserNameAsync(string userName);
    }
}