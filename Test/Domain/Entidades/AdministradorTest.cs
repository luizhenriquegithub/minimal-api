using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiminalApi.Dominio.Entidades;


namespace Test.Domain.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            //Arrange
            var adm = new Administrador();

            //Act => Set
            adm.Id = 1;
            adm.Email = "teste@teste.com.br";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            //Acert => Get
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("teste@teste.com.br", adm.Email);
            Assert.AreEqual("teste", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}