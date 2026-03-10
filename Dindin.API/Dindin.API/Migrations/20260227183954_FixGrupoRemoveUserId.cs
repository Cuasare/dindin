using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dindin.API.Migrations
{
    /// <inheritdoc />
    public partial class FixGrupoRemoveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupo_Users_UserId",
                table: "Grupo");

            migrationBuilder.DropIndex(
                name: "IX_Grupo_UserId",
                table: "Grupo");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Grupo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Grupo",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_UserId",
                table: "Grupo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupo_Users_UserId",
                table: "Grupo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
