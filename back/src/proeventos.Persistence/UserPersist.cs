using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain.Identity;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;

namespace proeventos.Persistence
{
    public class UserPersistence : GeralPersistence, IUserPersistence
    {
        private readonly proeventosContext _context;
        public UserPersistence(proeventosContext context) : base(context)
        {
            _context = context;

        }
        public async Task<IEnumerable<User>> GerUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GerUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                                 .SingleOrDefaultAsync(u => u.UserName == userName.ToLower());
        }

    }
}