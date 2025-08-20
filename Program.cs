
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Miminal.Dominio.Servicos;
using Miminal.DTOs;
using miminal_api.Dominio.Interfaces;
using MiminalApi.Infraestrutura.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(Options =>
{
    Options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
        );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World, Luiz Henrique");

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico AdministradorServico) =>
{
    if (AdministradorServico.Login(loginDTO) != null)
        return Results.Ok("Login feito com sucesso");
    else
        return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


