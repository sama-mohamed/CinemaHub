using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaWebSite.Migrations
{
    /// <inheritdoc />
    public partial class test5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorMovies",
                table: "ActorMovies");

            migrationBuilder.DropIndex(
                name: "IX_ActorMovies_ActorId",
                table: "ActorMovies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActorMovies");

            migrationBuilder.AlterColumn<string>(
                name: "CinemaLogo",
                table: "Cinemas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorMovies",
                table: "ActorMovies",
                columns: new[] { "ActorId", "MovieId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorMovies",
                table: "ActorMovies");

            migrationBuilder.AlterColumn<string>(
                name: "CinemaLogo",
                table: "Cinemas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ActorMovies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorMovies",
                table: "ActorMovies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovies_ActorId",
                table: "ActorMovies",
                column: "ActorId");
        }
    }
}
