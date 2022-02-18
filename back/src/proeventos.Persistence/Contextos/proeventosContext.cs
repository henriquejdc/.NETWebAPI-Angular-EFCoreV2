using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using proeventos.Domain;
using proeventos.Domain.Identity;

namespace proeventos.Persistence.Contextos
{
    public class proeventosContext : IdentityDbContext<User, Role, int,
                                     IdentityUserClaim<int>, UserRole, 
                                     IdentityUserLogin<int>, IdentityRoleClaim<int>, 
                                     IdentityUserToken<int>>
    {
        public proeventosContext(DbContextOptions<proeventosContext> options) : base(options){}
        public DbSet<Evento> Eventos {get; set;}
        public DbSet<Lote> Lotes {get; set;}
        public DbSet<Palestrante> Palestrantes {get; set;}
        public DbSet<PalestranteEvento> PalestranteEventos {get; set;}
        public DbSet<RedeSocial> RedesSociais {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserRole>(userRole => 
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            });


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