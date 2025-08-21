
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miminal.Dominio.Servicos;
using Miminal.DTOs;
using miminal_api.Dominio.DTOs;
using miminal_api.Dominio.Interfaces;
using miminal_api.Dominio.ModelViews;
using miminal_api.Dominio.Servicos;
using miminal_api.Dominio.Validador;
using MiminalApi.Dominio.Entidades;
using MiminalApi.Infraestrutura.DB;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

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
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico AdministradorServico) =>
{
    ArgumentNullException.ThrowIfNull(AdministradorServico);
    ArgumentNullException.ThrowIfNull(loginDTO);
    if (AdministradorServico.Login(loginDTO) != null)
        return Results.Ok("Login feito com sucesso");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };

    var erros = VeiculoValidador.Validar(veiculoDTO);
    if (erros.Any())
        return Results.BadRequest(new { Erros = erros });

    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);

}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int pagina, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.Todos(pagina);
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null) Results.NotFound();

    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null) Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    var erros = VeiculoValidador.Validar(veiculoDTO);
    if (erros.Any())
        return Results.BadRequest(new { Erros = erros });

    veiculoServico.Atualizar(veiculo);
    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapDelete("veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null) Results.NotFound();

    veiculoServico.Apagar(veiculo);
    return Results.NoContent();

}).WithTags("Veiculos");

#endregion

#region Swagger
app.UseSwagger();
app.UseSwaggerUI();
#endregion

app.Run();

