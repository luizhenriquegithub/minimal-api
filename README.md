---

# Trabalhando com Minimal APIs - ASP.NET Core

Este repositório contém um conjunto de exemplos e guias práticos para a criação de APIs minimalistas utilizando **ASP.NET Core**. 
A ideia é explorar o uso de Minimal APIs para acelerar o desenvolvimento de APIs RESTful, focando em simplicidade, performance e boas práticas.

## Funcionalidades

- Criação e consumo de rotas HTTP (GET, POST, PUT, DELETE) com Minimal APIs.
- Integração com **Entity Framework Core** para persistência de dados.
- Autenticação e autorização usando **JWT (JSON Web Tokens)**.
- Middleware personalizados para controle de requisições.
- Implementação de **Testes de Unidade** e **Testes de Integração** para validar funcionalidades.

- Otimização de performance com boas práticas para produção.

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET 8 SDK ou superior](https://dotnet.microsoft.com/download/dotnet/8.0)
- Um gerenciador de pacotes como **NuGet** para restaurar dependências

## Dependências do projeto

- Microsoft.AspNetCore.Authentication.JwtBearer      8.0.0        
- Microsoft.EntityFrameworkCore                      8.0.19       
- Microsoft.EntityFrameworkCore.Design               8.0.19       
- Microsoft.EntityFrameworkCore.InMemory             8.0.19       
- Microsoft.EntityFrameworkCore.Tools                8.0.19       
- Pomelo.EntityFrameworkCore.MySql                   8.0.3        
- Swashbuckle.AspNetCore                             9.0.3        

## Configuração do Projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/luizhenriquegithub/minimal-api.git
   ```

2. Navegue até o diretório do projeto:
   ```bash
   cd api
   ```

3. Restaure as dependências:
   ```bash
   dotnet restore
   ```

4. Configure a conexão com o banco de dados no arquivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=MinimalApiDb;User Id=seu_usuario;Password=sua_senha;"
   }
   ```

5. Execute as migrações do banco de dados:
   ```bash
   dotnet ef database update
   ```

6. Execute a aplicação:
   ```bash
   dotnet run
   ```

## Autenticação JWT

Este projeto implementa autenticação com **JWT**. Para testar as rotas protegidas, siga os passos abaixo:

1. Crie um usuário [Adminstrador] ou faça login na rota `/Administrador/login` e obtenha o token JWT.

2. Use o token JWT nas requisições às rotas protegidas passando-o no cabeçalho `Authorization`:
   ```
   Authorization: Bearer <seu_token_jwt>
   ```

## Rodando os Testes

Os testes de unidade e integração estão localizados na pasta `Tests`. Para rodá-los, utilize o seguinte comando:

```bash
dotnet test
```

## Licença

Este projeto é licenciado sob a [MIT License](LICENSE).

