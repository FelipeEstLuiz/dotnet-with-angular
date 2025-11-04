using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_main",
                table: "photos");

            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "aspnet_users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "aspnet_users");

            migrationBuilder.AddColumn<bool>(
                name: "is_main",
                table: "photos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
