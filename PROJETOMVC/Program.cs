using System;
using Microsoft.EntityFrameworkCore;
using PROJETOMVC.Data;
using PROJETOMVC.Models;
using PROJETOMVC.Repositorio;

var builder = WebApplication.CreateBuilder(args);

//dbcontext
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// ✅ ADICIONE A CONFIGURAÇÃO DE SESSÃO:
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sessão expira em 30 minutos
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// ✅ ADICIONE O MIDDLEWARE DE SESSÃO (ANTES DE UseAuthorization):
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}") // ✅ MUDADO PARA Login
    .WithStaticAssets();

app.Run();