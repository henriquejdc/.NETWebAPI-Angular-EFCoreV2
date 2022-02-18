using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Persistence.Contratos;

namespace proeventos.Persistence
{
    public class GeralPersistence : IGeralPersistence
    {
        private readonly proeventosContext _context;
        public GeralPersistence(proeventosContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}