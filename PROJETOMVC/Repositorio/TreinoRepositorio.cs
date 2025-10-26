using AcademiaApp.Data;
using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJETOMVC.Repositorio
{
    public class TreinoRepositorio : ITreinoRepositorio
    {
        private readonly AcademiaContext _context;

        public TreinoRepositorio(AcademiaContext context)
        {
            _context = context;
        }

        // CRUD Básico
        public async Task<Treino?> BuscarPorIdAsync(int id)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Treino>> BuscarTodosAsync()
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<Treino> AdicionarAsync(Treino treino)
        {
            treino.DataCriacao = DateTime.UtcNow;

            // Converte datas para UTC se existirem
            if (treino.DataInicio.HasValue)
            {
                treino.DataInicio = DateTime.SpecifyKind(treino.DataInicio.Value, DateTimeKind.Utc);
            }

            if (treino.DataFim.HasValue)
            {
                treino.DataFim = DateTime.SpecifyKind(treino.DataFim.Value, DateTimeKind.Utc);
            }

            await _context.Treinos.AddAsync(treino);
            await _context.SaveChangesAsync();
            return treino;
        }

        public async Task<Treino> AtualizarAsync(Treino treino)
        {
            var treinoBanco = await BuscarPorIdAsync(treino.Id);

            if (treinoBanco == null)
                throw new Exception("Treino não encontrado!");

            // Atualiza os campos
            treinoBanco.Nome = treino.Nome;
            treinoBanco.Descricao = treino.Descricao;
            treinoBanco.Nivel = treino.Nivel;
            treinoBanco.Tipo = treino.Tipo;
            treinoBanco.DataInicio = treino.DataInicio;
            treinoBanco.DataFim = treino.DataFim;
            treinoBanco.Ativo = treino.Ativo;
            treinoBanco.UsuarioId = treino.UsuarioId;
            treinoBanco.CriadoPorId = treino.CriadoPorId;

            // Converte datas para UTC
            if (treinoBanco.DataInicio.HasValue)
            {
                treinoBanco.DataInicio = DateTime.SpecifyKind(treinoBanco.DataInicio.Value, DateTimeKind.Utc);
            }

            if (treinoBanco.DataFim.HasValue)
            {
                treinoBanco.DataFim = DateTime.SpecifyKind(treinoBanco.DataFim.Value, DateTimeKind.Utc);
            }

            _context.Treinos.Update(treinoBanco);
            await _context.SaveChangesAsync();
            return treinoBanco;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var treinoBanco = await BuscarPorIdAsync(id);

            if (treinoBanco == null)
                return false;

            _context.Treinos.Remove(treinoBanco);
            await _context.SaveChangesAsync();
            return true;
        }

        // Buscas Específicas
        public async Task<IEnumerable<Treino>> BuscarPorUsuarioAsync(int usuarioId)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Treino>> BuscarPorTreinadorAsync(int treinadorId)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Where(t => t.CriadoPorId == treinadorId)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Treino>> BuscarAtivosAsync()
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Where(t => t.Ativo)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Treino>> BuscarPorNivelAsync(NivelDificuldade nivel)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Where(t => t.Nivel == nivel)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Treino>> BuscarPorTipoAsync(TipoTreino tipo)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Where(t => t.Tipo == tipo)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        // Buscas com relacionamentos completos
        public async Task<Treino?> BuscarComExerciciosAsync(int id)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Include(t => t.TreinoExercicios)
                    .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Treino?> BuscarComExecucoesAsync(int id)
        {
            return await _context.Treinos
                .Include(t => t.Usuario)
                .Include(t => t.CriadoPor)
                .Include(t => t.Execucoes)
                    .ThenInclude(e => e.Usuario)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}