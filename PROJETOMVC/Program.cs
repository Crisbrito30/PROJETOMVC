using AcademiaApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Repositorio;

// outros usings...

var builder = WebApplication.CreateBuilder(args);

// DB, repositórios, sessão...


builder.Services.AddDbContext<AcademiaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<ITreinoRepositorio, TreinoRepositorio>();

builder.Services.AddScoped<IExercicioRepositorio, ExercicioRepositorio>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Configura autenticação por cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";       // para onde redirecionar se não autenticado
        options.LogoutPath = "/Login/Sair";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // em produção
    });

// Controllers + views
builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();     // certifique-se que os arquivos estáticos estão habilitados
app.UseRouting();

app.UseSession();         // sessão (se ainda quiser usar além do cookie)
app.UseAuthentication();  // importante: antes de UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
