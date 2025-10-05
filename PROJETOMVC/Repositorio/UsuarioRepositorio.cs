using PROJETOMVC.Data;
using PROJETOMVC.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace PROJETOMVC.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _bancoContext;

        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        // Adicionar Usuário
        public async Task<UsuarioModel> AdicionarAsync(UsuarioModel usuario)
        {
            if (await EmailExisteAsync(usuario.Email))
                throw new Exception("Erro: E-mail já cadastrado.");

            if (await LoginExisteAsync(usuario.Login))
                throw new Exception("Erro: Login já cadastrado.");

            usuario.DataCadastro = DateTime.UtcNow;
            usuario.Senha = GerarHashSenha(usuario.Senha);

            await _bancoContext.Usuarios.AddAsync(usuario);
            await _bancoContext.SaveChangesAsync();

            return usuario;
        }


        // Listar Todos os Usuários
        public async Task<List<UsuarioModel>> BuscarTodosAsync()
        {
            return await _bancoContext.Usuarios
                .AsNoTracking() // Melhora performance para leitura
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        // Listar por ID
        public async Task<UsuarioModel> ListarPorIdAsync(int id)
        {
            return await _bancoContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Atualizar Usuário
        public async Task<UsuarioModel> AtualizarAsync(UsuarioModel usuario)
        {
            var usuarioDB = await ListarPorIdAsync(usuario.Id);

            if (usuarioDB == null)
                throw new Exception("Erro: Usuário não encontrado.");

            // Valida se email já existe em outro usuário
            if (await EmailExisteAsync(usuario.Email, usuario.Id))
                throw new Exception("Erro: E-mail já cadastrado para outro usuário.");

            // Valida se login já existe em outro usuário
            if (await LoginExisteAsync(usuario.Login, usuario.Id))
                throw new Exception("Erro: Login já cadastrado para outro usuário.");

            // Atualiza apenas os campos permitidos
            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Perfil = usuario.Perfil;
            usuarioDB.DataAtualizacao = DateTime.UtcNow;

            // Só atualiza senha se foi informada
            if (!string.IsNullOrEmpty(usuario.Senha))
                usuarioDB.Senha = GerarHashSenha(usuario.Senha);

            await _bancoContext.SaveChangesAsync();

            return usuarioDB;
        }

        // Deletar Usuário
        public async Task<bool> DeletarAsync(int id)
        {
            var usuarioDB = await ListarPorIdAsync(id);

            if (usuarioDB == null)
                throw new Exception("Erro: Usuário não encontrado.");

            _bancoContext.Usuarios.Remove(usuarioDB);
            await _bancoContext.SaveChangesAsync();

            return true;
        }

        // Métodos auxiliares privados
        private async Task<bool> EmailExisteAsync(string email, int? usuarioId = null)
        {
            return await _bancoContext.Usuarios
                .AnyAsync(u => u.Email == email && u.Id != usuarioId);
        }

        private async Task<bool> LoginExisteAsync(string login, int? usuarioId = null)
        {
            return await _bancoContext.Usuarios
                .AnyAsync(u => u.Login == login && u.Id != usuarioId);
        }


        private string GerarHashSenha(string senha)
        {
            // Use BCrypt ou outra biblioteca de hash segura
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
    }
}