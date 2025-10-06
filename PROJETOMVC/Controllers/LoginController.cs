using Microsoft.AspNetCore.Mvc;
using PROJETOMVC.Models;
using PROJETOMVC.Repositorio;

namespace PROJETOMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public LoginController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // GET: Login/Index - Exibe tela de login
        public IActionResult Index()
        {
            // Se já estiver logado, redireciona
            if (HttpContext.Session.GetString("UsuarioLogado") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Login/Entrar - Processa login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioRepositorio.BuscarPorLoginAsync(loginModel.Login);

                    if (usuario != null)
                    {
                        // Valida senha com BCrypt
                        if (BCrypt.Net.BCrypt.Verify(loginModel.Senha, usuario.Senha))
                        {
                            // Salva sessão
                            HttpContext.Session.SetString("UsuarioLogado", usuario.Nome);
                            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                            HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil.ToString());

                            TempData["MensagemSucesso"] = $"Bem-vindo(a), {usuario.Nome}!";
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    TempData["MensagemErro"] = "Login ou senha inválidos!";
                }
                else
                {
                    TempData["MensagemErro"] = "Preencha todos os campos!";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao fazer login: {ex.Message}";
            }

            return View("Index");
        }

        // GET: Login/Sair - Faz logout
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            TempData["MensagemSucesso"] = "Logout realizado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}