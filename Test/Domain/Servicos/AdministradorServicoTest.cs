using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(databaseName: "Testedatabase").Options;

            return new DbContexto((Microsoft.Extensions.Configuration.IConfiguration)options);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            //Arrange
            var adm = new Administrador();
            adm.Email = "teste@teste.com.br";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var context = CriarContextoDeTeste();
            var administradorServico = new AdministradorServico(context);

            //Act => 
            administradorServico.Incluir(adm);

            //Acert => 
            Assert.AreEqual(1, administradorServico.Todos(1).Count());
        }

    }
}