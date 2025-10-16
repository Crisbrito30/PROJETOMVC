using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Models;
using PROJETOMVC.Models.Enums;

namespace AcademiaApp.Data
{
    public class AcademiaContext : DbContext
    {
        public AcademiaContext(DbContextOptions<AcademiaContext> options) : base(options)
        {
        }

        // DbSets - Tabelas do banco
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TreinoExercicio> TreinoExercicios { get; set; }
        public DbSet<ExecucaoTreino> ExecucoesTreino { get; set; }
        public DbSet<ExecucaoExercicio> ExecucoesExercicio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamentos e comportamentos

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Cpf)
                .IsUnique();

            // Treino - Relacionamento com Usuario (Aluno)
            modelBuilder.Entity<Treino>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.Treinos)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Treino - Relacionamento com Usuario (Treinador que criou)
            modelBuilder.Entity<Treino>()
                .HasOne(t => t.CriadoPor)
                .WithMany(u => u.TreinosCriados)
                .HasForeignKey(t => t.CriadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            // TreinoExercicio - Relacionamento com Treino
            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Treino)
                .WithMany(t => t.TreinoExercicios)
                .HasForeignKey(te => te.TreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            // TreinoExercicio - Relacionamento com Exercicio
            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Exercicio)
                .WithMany(e => e.TreinoExercicios)
                .HasForeignKey(te => te.ExercicioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExecucaoTreino - Relacionamento com Treino
            modelBuilder.Entity<ExecucaoTreino>()
                .HasOne(et => et.Treino)
                .WithMany(t => t.Execucoes)
                .HasForeignKey(et => et.TreinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExecucaoTreino - Relacionamento com Usuario (Aluno)
            modelBuilder.Entity<ExecucaoTreino>()
                .HasOne(et => et.Usuario)
                .WithMany(u => u.ExecucoesTreino)
                .HasForeignKey(et => et.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExecucaoExercicio - Relacionamento com ExecucaoTreino
            modelBuilder.Entity<ExecucaoExercicio>()
                .HasOne(ee => ee.ExecucaoTreino)
                .WithMany(et => et.ExecucaoExercicios)
                .HasForeignKey(ee => ee.ExecucaoTreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ExecucaoExercicio - Relacionamento com TreinoExercicio
            modelBuilder.Entity<ExecucaoExercicio>()
                .HasOne(ee => ee.TreinoExercicio)
                .WithMany()
                .HasForeignKey(ee => ee.TreinoExercicioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurações de precisão para campos decimais
            modelBuilder.Entity<TreinoExercicio>()
                .Property(te => te.Carga)
                .HasPrecision(6, 2);

            modelBuilder.Entity<ExecucaoExercicio>()
                .Property(ee => ee.CargaUtilizada)
                .HasPrecision(6, 2);

            // Dados iniciais (Seed Data) - Opcional
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Você pode adicionar dados iniciais aqui
            // Exemplo: usuário administrador padrão, exercícios básicos, etc.

            // Exemplo de usuário administrador inicial
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "admin@academia.com",
                    Cpf = "000.000.000-00",
                    PerfilUser = PerfilUser.Administrador,
                    Senha = "admin123", // Lembre-se de usar hash em produção!
                    DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Ativo = true
                }
            );

            // Exemplo de exercícios básicos
            modelBuilder.Entity<Exercicio>().HasData(
                new Exercicio { Id = 1, Nome = "Supino Reto", GrupoMuscular = GrupoMuscular.Peito, Ativo = true },
                new Exercicio { Id = 2, Nome = "Agachamento Livre", GrupoMuscular = GrupoMuscular.Pernas, Ativo = true },
                new Exercicio { Id = 3, Nome = "Desenvolvimento", GrupoMuscular = GrupoMuscular.Ombros, Ativo = true },
                new Exercicio { Id = 4, Nome = "Remada Curvada", GrupoMuscular = GrupoMuscular.Costas, Ativo = true },
                new Exercicio { Id = 5, Nome = "Rosca Direta", GrupoMuscular = GrupoMuscular.Biceps, Ativo = true },
                new Exercicio { Id = 6, Nome = "Tríceps Testa", GrupoMuscular = GrupoMuscular.Triceps, Ativo = true },
                new Exercicio { Id = 7, Nome = "Leg Press", GrupoMuscular = GrupoMuscular.Pernas, Ativo = true },
                new Exercicio { Id = 8, Nome = "Abdominal Crunch", GrupoMuscular = GrupoMuscular.Abdomen, Ativo = true }
            );
        }
    }
}