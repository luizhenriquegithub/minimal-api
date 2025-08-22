using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miminal_api.Dominio.ModelViews
{
    public struct Home
    {
        public string Mensagem { get => "Bem vindo a API de Veiculos"; }
        public string DocumentoAPI { get => "/swagger"; }
    }
}