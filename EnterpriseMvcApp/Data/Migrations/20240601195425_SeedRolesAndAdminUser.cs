using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseMvcApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[] { new Guid("cc07b7ff-f4c0-40e8-9762-7dc8374f214f"), new DateTime(2024, 6, 1, 19, 54, 25, 79, DateTimeKind.Utc).AddTicks(1629), "admin@example.com", "$2a$11$ShqJC20BrUIjBXZfzeOSjOGWrqr8cwY8xO8O8.SSGWPzsuAAFhKlO", "Admin", new DateTime(2024, 6, 1, 19, 54, 25, 79, DateTimeKind.Utc).AddTicks(1633), "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cc07b7ff-f4c0-40e8-9762-7dc8374f214f"));
        }
    }
}
