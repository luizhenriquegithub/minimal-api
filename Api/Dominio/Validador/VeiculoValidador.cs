using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using miminal_api.Dominio.DTOs;

namespace miminal_api.Dominio.Validador
{
    public static class VeiculoValidador
    {
        public static List<string> Validar(VeiculoDTO veiculo)
        {
            var erros = new List<String>();

            if (string.IsNullOrEmpty(veiculo.Nome))
                erros.Add("Nome é obrigatorio");

            if (string.IsNullOrEmpty(veiculo.Marca))
                erros.Add("Marca é obrigatoria");

            if (veiculo.Ano < 1900 || veiculo.Ano > DateTime.Now.Year + 1)
                erros.Add("Ano deve estar entre 1900 e o proximo ano");

            return erros;
        }
    }
}