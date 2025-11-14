using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "access_failed_count",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "lockout_enabled",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "lockout_end",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "phone_number_confirmed",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "security_stamp",
                table: "aspnet_users");

            migrationBuilder.DropColumn(
                name: "two_factor_enabled",
                table: "aspnet_users");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "aspnet_users",
                newName: "EmailConfirmed");

            migrationBuilder.CreateTable(
                name: "user_like",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "int", nullable: false),
                    TargetUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_like", x => new { x.SourceUserId, x.TargetUserId });
                    table.ForeignKey(
                        name: "FK_user_like_aspnet_users_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "aspnet_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_like_aspnet_users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "aspnet_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_like_TargetUserId",
                table: "user_like",
                column: "TargetUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_like");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "aspnet_users",
                newName: "email_confirmed");

            migrationBuilder.AddColumn<int>(
                name: "access_failed_count",
                table: "aspnet_users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "concurrency_stamp",
                table: "aspnet_users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "lockout_enabled",
                table: "aspnet_users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lockout_end",
                table: "aspnet_users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "aspnet_users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "phone_number_confirmed",
                table: "aspnet_users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "security_stamp",
                table: "aspnet_users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "two_factor_enabled",
                table: "aspnet_users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
