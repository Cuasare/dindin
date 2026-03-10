using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dindin.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CodigoConvite = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Depositos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MetodoDeposito = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depositos_Grupo_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Grupo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Depositos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MetodoPagamento = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    AddAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrupoId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_Grupo_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Grupo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gastos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGrupo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GrupoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGrupo", x => new { x.UserId, x.GrupoId });
                    table.ForeignKey(
                        name: "FK_UserGrupo_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGrupo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_GroupId",
                table: "Depositos",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_UserId",
                table: "Depositos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_GroupId",
                table: "Gastos",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_UserId",
                table: "Gastos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_UserId",
                table: "Grupo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGrupo_GrupoId",
                table: "UserGrupo",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depositos");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserGrupo");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
