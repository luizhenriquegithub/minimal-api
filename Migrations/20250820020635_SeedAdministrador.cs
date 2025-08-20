using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miminal_api.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdministrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "administradores",
                columns: new[] { "Id", "Email", "Perfiel", "Senha" },
                values: new object[] { 1, "admin@teste.com", "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "administradores",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
