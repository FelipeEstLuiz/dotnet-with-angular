using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "aspnet_users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "aspnet_users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
