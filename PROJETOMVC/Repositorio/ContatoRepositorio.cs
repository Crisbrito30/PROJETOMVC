using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Data;
using PROJETOMVC.Models;

namespace PROJETOMVC.Repositorio
{
    public class ContatoRepositorio : IContatoRepositorio
    {
        private readonly BancoContext _bancoContext;

        public ContatoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        // ✅ Adicionar contato
        public async Task<ContatoModel> AdicionarAsync(ContatoModel contato)
        {
            await _bancoContext.Contatos.AddAsync(contato);
            await _bancoContext.SaveChangesAsync();
            return contato;
        }

        // ✅ Buscar todos
        public async Task<List<ContatoModel>> BuscarTodosAsync()
        {
            return await _bancoContext.Contatos
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        // ✅ Buscar por ID
        public async Task<ContatoModel> ListarPorIdAsync(int id)
        {
            return await _bancoContext.Contatos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ✅ Atualizar
        public async Task<ContatoModel> AtualizarAsync(ContatoModel contato)
        {
            var contatoDB = await ListarPorIdAsync(contato.Id);

            if (contatoDB == null)
                throw new Exception("Erro: contato não encontrado.");

            contatoDB.Nome = contato.Nome;
            contatoDB.Sobrenome = contato.Sobrenome;
            contatoDB.Email = contato.Email;

            _bancoContext.Contatos.Update(contatoDB);
            await _bancoContext.SaveChangesAsync();

            return contatoDB;
        }

        // ✅ Deletar
        public async Task<bool> DeletarAsync(int id)
        {
            var contatoDB = await ListarPorIdAsync(id);

            if (contatoDB == null)
                throw new Exception("Erro: contato não encontrado.");

            _bancoContext.Contatos.Remove(contatoDB);
            await _bancoContext.SaveChangesAsync();

            return true;
        }
    }
}
