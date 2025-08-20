using Microsoft.EntityFrameworkCore;
using Miminal.DTOs;
using miminal_api.Dominio.Interfaces;
using MiminalApi.Dominio.Entidades;
using MiminalApi.Infraestrutura.DB;

namespace Miminal.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;

    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }
}