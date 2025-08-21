using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using miminal_api.Dominio.Enuns;

namespace miminal_api.Dominio.ModelViews
{
    public record AdmModelView
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Perfil Perfil { get; set; }
    }
}