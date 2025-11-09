using AcademiaApp.Models;
using System.Data;

namespace PROJETOMVC.Models
{
    public class UsuarioFiltroViewModel
    {
        //Filtros
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? PerfilUser { get; set; }
        public DateTime? dataInical { get; set; }
        public DateTime? dataFinal { get; set; }

        //paginação
        public int PaginaAtual { get; set; } = 1;
        public int ItensPorPagina { get; set; } = 10;
        public int TotalItens { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((decimal)TotalItens / ItensPorPagina);

        //Ordenação
        public string OrdenarPor { get; set; } = "Nome";
        public string ordernarDirecao { get; set; } = "ASC";

        //Lista de Usuários
        public List<UsuarioModel>? Usuarios { get; set; } = new List<UsuarioModel>();

        //Propriedade auxiliares
        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => PaginaAtual < TotalPaginas;
        public int PaginaInicio => Math.Max(1, PaginaAtual - 2);
        public int PaginaFim => Math.Min(TotalPaginas, PaginaAtual + 2);
    }
    }
