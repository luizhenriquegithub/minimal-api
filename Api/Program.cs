
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Miminal.Dominio.Servicos;
using Miminal.DTOs;
using miminal_api.Dominio.DTOs;
using miminal_api.Dominio.Enuns;
using miminal_api.Dominio.Interfaces;
using miminal_api.Dominio.ModelViews;
using miminal_api.Dominio.Servicos;
using miminal_api.Dominio.Validador;
using MiminalApi.Dominio.Entidades;
using MiminalApi.Infraestrutura.DB;

#region Builder
var builder = WebApplication.CreateBuilder(args);

//Jwt
var chave = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(chave)) chave = "1234567";

//Jwt
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Jwt
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(Options =>
{
    Options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT aqui:"
    });

    Options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administradores

//Jwt
string GeraTokenJtw(Administrador administrador)
{
    if (string.IsNullOrEmpty(chave)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email" , administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role , administrador.Perfil)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico AdministradorServico) =>
{
    var adm = AdministradorServico.Login(loginDTO);
    if (adm != null)
    {
        string token = GeraTokenJtw(adm);

        return Results.Ok(new AdministradorLogado
        {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    else
        return Results.Unauthorized();

}).AllowAnonymous().WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    var erros = AdmValidador.Validar(administradorDTO);
    if (erros.Any())
        return Results.BadRequest(new { Erros = erros });

    var administrador = new Administrador
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString()
    };

    administradorServico.Incluir(administrador);
    return Results.Created($"/administrador/{administrador}", administrador);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int pagina, IAdministradorServico administradorServico) =>
{
    var adms = new List<AdmModelView>();
    var administradores = administradorServico.Todos(pagina);
    foreach (var adm in administradores)
    {
        adms.Add(new AdmModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }
    return Results.Ok(adms);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscaPorId(id);
    if (administrador == null) return Results.NotFound();

    return Results.Ok(administrador);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

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

}).RequireAuthorization()
  .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
  .WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int pagina, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.Todos(pagina);
    return Results.Ok(veiculo);

}).RequireAuthorization()
  .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
  .WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null) Results.NotFound();

    return Results.Ok(veiculo);

}).RequireAuthorization()
  .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
  .WithTags("Veiculos");

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

}).RequireAuthorization()
  .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
  .WithTags("Veiculos");

app.MapDelete("veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null) Results.NotFound();

    veiculoServico.Apagar(veiculo);
    return Results.NoContent();

}).RequireAuthorization()
  .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
  .WithTags("Veiculos");

#endregion

#region Swagger
app.UseSwagger();
app.UseSwaggerUI();
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.Run();

