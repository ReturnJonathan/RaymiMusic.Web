using Microsoft.EntityFrameworkCore;
using RaymiMusic.Modelos;

namespace RaymiMusic.Data
{
    public class UsuariosDbContext : DbContext
    {
        public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Artista> Artistas { get; set; }
    }
}
