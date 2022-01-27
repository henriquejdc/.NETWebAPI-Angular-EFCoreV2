using System.Threading.Tasks;
using proeventos.Domain;

namespace proeventos.Persistence.Contratos
{
    public interface IGeralPersistence
    {
        //Metodos Gerais
         void Add<T>(T entity) where T: class;
         void Update<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         void DeleteRange<T>(T[] entity) where T: class;

         Task<bool> SaveChangesAsync();

    }
}