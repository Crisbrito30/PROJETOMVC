
using AcademiaApp.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Repositorio;

// 1) Descobrir/forçar o ambiente via variável do processo (terminal/launchSettings)
var envFromProcess = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                    ?? "Production"; // default se nada estiver definido

// 2) Carregar o .env correto ANTES de criar o builder
if (string.Equals(envFromProcess, "Development", StringComparison.OrdinalIgnoreCase))
{
    Env.Load(".env.development");
}
else
{
    Env.Load(".env.production");
}

// 3) Agora crie o builder (o host já verá as variáveis carregadas)
var builder = WebApplication.CreateBuilder(args);

// Logs de verificação (sem caracteres de escape!)
var currentEnv = builder.Environment.EnvironmentName;
var connString = builder.Configuration.GetConnectionString("DataBase");

Console.WriteLine($"[CHECK] Ambiente: {currentEnv}");
Console.WriteLine($"[CHECK] Connection String: {connString}");

// 4) Serviços
builder.Services.AddDbContext<AcademiaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<ITreinoRepositorio, TreinoRepositorio>();
builder.Services.AddScoped<IExercicioRepositorio, ExercicioRepositorio>();
builder.Services.AddScoped<IDashboardRepositorio, DashboardRepositorio>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Sair";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        // Em produção, considere:
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddControllersWithViews();

// 5) Pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
