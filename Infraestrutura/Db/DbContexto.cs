using Microsoft.EntityFrameworkCore;
using MiminalApi.Dominio.Entidades;

namespace MiminalApi.Infraestrutura.DB;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuracaoAppSettings;

    public DbContexto(IConfiguration configuracaoAppSettings)
    {
        _configuracaoAppSettings = configuracaoAppSettings;
    }

    public DbSet<Administrador> administradores { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador
            {
                Id = 1,
                Email = "admin@teste.com",
                Senha = "admin",
                Perfiel = "admin"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var stringConexao = _configuracaoAppSettings.GetConnectionString("mysql").ToString();

            if (!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
            }
        }
    }
}