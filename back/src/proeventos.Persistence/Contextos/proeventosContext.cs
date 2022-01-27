using Microsoft.EntityFrameworkCore;
using proeventos.Domain;

namespace proeventos.Persistence.Contextos
{
    public class proeventosContext : DbContext
    {
        public proeventosContext(DbContextOptions<proeventosContext> options) : base(options){}
        public DbSet<Evento> Eventos {get; set;}
        public DbSet<Lote> Lotes {get; set;}
        public DbSet<Palestrante> Palestrantes {get; set;}
        public DbSet<PalestranteEvento> PalestranteEventos {get; set;}
        public DbSet<RedeSocial> RedesSociais {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            
            modelBuilder.Entity<PalestranteEvento>()
            .HasKey(pe => new {pe.EventoId, pe.PalestranteId});
            
            modelBuilder.Entity<Evento>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Evento)
            .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Palestrante>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Palestrante)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}