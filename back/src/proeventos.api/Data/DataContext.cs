using Microsoft.EntityFrameworkCore;
using proeventos.api.Models;

namespace proeventos.api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<Evento> Eventos {get; set;}
    }
}