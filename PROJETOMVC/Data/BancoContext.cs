using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Models;

namespace PROJETOMVC.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<ContatoModel> Contatos { get; set; }

        public   DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContatoModel>()
                .Property(c => c.Nascimento)
                .HasColumnType("date");

            // ✅ Ajuste para o UsuarioModel
            modelBuilder.Entity<UsuarioModel>()
                .Property(u => u.DataCadastro)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<UsuarioModel>()
                .Property(u => u.DataAtualizacao)
                .HasColumnType("timestamp with time zone");

            // ✅ Faz o EF entender que PerfilEnum é salvo como int
            modelBuilder.Entity<UsuarioModel>()
                .Property(u => u.Perfil)
                .HasConversion<int>();

            base.OnModelCreating(modelBuilder);
        }





    }
}
   
