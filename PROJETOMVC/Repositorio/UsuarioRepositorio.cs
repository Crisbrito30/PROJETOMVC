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

        public async Task<UsuarioModel?> BuscarPorLoginAsync(string emailOuLogin)
        {
            // Aqui você pode usar login, email ou cpf dependendo de como o login é feito
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == emailOuLogin || u.Cpf == emailOuLogin);
        }

        public async Task<UsuarioModel?> BuscarPorIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<UsuarioModel>> BuscarTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> AdicionarAsync(UsuarioModel usuario)
        {
            //Verifica se o CPF já está cadastrado
            var cpfExistente = await _context.Usuarios
                .AnyAsync(u => u.Cpf == usuario.Cpf);
            if (cpfExistente)
            {
                throw new Exception("CPF já cadastrado!");
            }
            //Verifica se o email já está cadastrado
            var emailExistente = await _context.Usuarios
                .AnyAsync(u => u.Email == usuario.Email);
            if (emailExistente)
            {
                throw new Exception("Email já cadastrado no sistema!");
            }

            // Define a data de cadastro como UTC
            usuario.DataCadastro = DateTime.UtcNow;

            // Criptografa a senha
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            // Converte todas as datas opcionais para UTC, se existirem
            usuario.DataNascimento = ToUtc(usuario.DataNascimento);
            usuario.DataMatricula = ToUtc(usuario.DataMatricula);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioModel> AtualizarAsync(UsuarioModel usuario)
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

        public async Task<UsuarioModel> ListarPorIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception("Usuário não encontrado!");
        }


        // Função auxiliar para converter uma data para UTC
        private static DateTime? ToUtc(DateTime? data)
        {
            if (!data.HasValue) return null;

            return DateTime.SpecifyKind(data.Value, DateTimeKind.Utc);
        }

        //Buscar com filtro e paginação 
        public async Task<(List<UsuarioModel> usuarios, int total)> BuscarComFiltrosAsync(
            string? nome,
            string? email,
            string? cpf,
            string? perfil,
            DateTime? dataInicio,
            DateTime? dataFim,
            int pagina,
            int itensPorPagina,
            string ordenarPor,
            string direcaoOrdem)
        {
            var query = _context.Usuarios.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(u => u.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(cpf))
                query = query.Where(u => u.Cpf.Contains(cpf));

            if (!string.IsNullOrWhiteSpace(perfil))
            {
                if (int.TryParse(perfil, out int perfilInt))
                    query = query.Where(u => (int)u.PerfilUser == perfilInt);
            }

            if (dataInicio.HasValue)
                query = query.Where(u => u.DataCadastro.Date >= dataInicio.Value.Date);

            if (dataFim.HasValue)
                query = query.Where(u => u.DataCadastro.Date <= dataFim.Value.Date);

            // Total de registros (antes da paginação)
            var total = await query.CountAsync();

            // Aplicar ordenação
            query = ordenarPor?.ToLower() switch
            {
                "nome" => direcaoOrdem == "desc" ? query.OrderByDescending(u => u.Nome) : query.OrderBy(u => u.Nome),
                "email" => direcaoOrdem == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                //"login" => direcaoOrdem == "desc" ? query.OrderByDescending(u => u.Login) : query.OrderBy(u => u.Login),
                "data" => direcaoOrdem == "desc" ? query.OrderByDescending(u => u.DataCadastro) : query.OrderBy(u => u.DataCadastro),
                _ => query.OrderBy(u => u.Nome)
            };

            // Aplicar paginação
            var usuarios = await query
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .AsNoTracking()
                .ToListAsync();

            return (usuarios, total);
        }
    }
}
