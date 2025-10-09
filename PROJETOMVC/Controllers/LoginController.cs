using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJETOMVC.Models;
using PROJETOMVC.Repositorio;
using System.Security.Claims;
// outros usings...

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    public LoginController(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UsuarioLogado") != null)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Entrar(LoginModel loginModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepositorio.BuscarPorLoginAsync(loginModel.Login);
                if (usuario != null && BCrypt.Net.BCrypt.Verify(loginModel.Senha, usuario.Senha))
                {
                    // Cria claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nome),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Opcional: ainda gravar na sessão se precisar
                    HttpContext.Session.SetString("UsuarioLogado", usuario.Nome);
                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                    HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil.ToString());

                    TempData["MensagemSucesso"] = $"Bem-vindo(a), {usuario.Nome}!";
                    return RedirectToAction("Index", "Home");
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

    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        TempData["MensagemSucesso"] = "Logout realizado com sucesso!";
        return RedirectToAction("Index");
    }
}
