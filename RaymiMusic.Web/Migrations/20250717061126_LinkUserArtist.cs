using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaymiMusic.Api.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserArtist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Artistas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Artistas_UsuarioId",
                table: "Artistas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artistas_Usuarios_UsuarioId",
                table: "Artistas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artistas_Usuarios_UsuarioId",
                table: "Artistas");

            migrationBuilder.DropIndex(
                name: "IX_Artistas_UsuarioId",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Artistas");
        }
    }
}
