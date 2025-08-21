using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using miminal_api.Dominio.DTOs;

namespace miminal_api.Dominio.Validador
{
    public static class AdmValidador
    {
        public static List<String> Validar(AdministradorDTO administrador)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(administrador.Email))
                erros.Add("Email é obrigadorio");

            if (string.IsNullOrWhiteSpace(administrador.Senha))
                erros.Add("Senha é obrigatorio");

            if (string.IsNullOrEmpty(administrador.Perfil.ToString()))
                erros.Add("Perfil é obrigatorio");

            return erros;
        }

    }
}