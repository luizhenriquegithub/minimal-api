using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Miminal.Dominio.Servicos;
using MiminalApi.Dominio.Entidades;
using MiminalApi.Infraestrutura.DB;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {

        private DbContexto CriarContextoDeTeste()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            var Configuration = builder.Build();
            return new DbContexto(Configuration);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            //Arrange
            var adm = new Administrador();
            adm.Email = "teste@mais.com";
            adm.Senha = "teste";
            adm.Perfil = "Editor";

            var context = CriarContextoDeTeste();
            var administradorServico = new AdministradorServico(context);

            //Act => 
            administradorServico.Incluir(adm);

            //Acert => 
            Assert.AreEqual(administradorServico.Todos(1).Count(), administradorServico.Todos(1).Count());
        }

    }
}