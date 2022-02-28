using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace proeventos.api.helpers
{
    public interface IUtil
    {         
        Task<string> SaveImage(IFormFile imageFile, string destino);
        void DeleteImage(string imageName, string destino);
    }
}