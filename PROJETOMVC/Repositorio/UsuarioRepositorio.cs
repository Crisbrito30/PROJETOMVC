using AcademiaApp.Data;
using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore;


namespace PROJETOMVC.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AcademiaContext _context;

        public UsuarioRepositorio(AcademiaContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorLoginAsync(string emailOuLogin)
        {
            // Aqui você pode usar login, email ou cpf dependendo de como o login é feito
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == emailOuLogin || u.Cpf == emailOuLogin);
        }

        public async Task<Usuario?> BuscarPorIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> BuscarTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha); // Criptografa a senha
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> AtualizarAsync(Usuario usuario)
        {
            var usuarioBanco = await BuscarPorIdAsync(usuario.Id);

            if (usuarioBanco == null) throw new Exception("Usuário não encontrado!");

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Telefone = usuario.Telefone;
            usuarioBanco.PerfilUser = usuario.PerfilUser;
            usuarioBanco.Ativo = usuario.Ativo;

            if (!string.IsNullOrEmpty(usuario.Senha))
                usuarioBanco.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            _context.Usuarios.Update(usuarioBanco);
            await _context.SaveChangesAsync();

            return usuarioBanco;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var usuarioBanco = await BuscarPorIdAsync(id);
            if (usuarioBanco == null) return false;

            _context.Usuarios.Remove(usuarioBanco);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario> ListarPorIdAsync(int id)
        {
           return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id) 
               ?? throw new Exception("Usuário não encontrado!");
        }
    }
}
