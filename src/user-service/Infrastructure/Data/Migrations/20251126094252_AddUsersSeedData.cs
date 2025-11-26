using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShrinkLink.UserService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773"), "admin@admin.com", "AQAAAAIAAYagAAAAEDliq5Roxa0gkptym2OdPjNXO4oKQX8XFRjXeN+2wUUjzOE4Uo3swfvqZwljE/In/w==" },
                    { new Guid("53f72fc2-cbda-43fe-90b9-45ed571e4185"), "user@user.com", "AQAAAAIAAYagAAAAEEl9XcH2utjEVsSK57jhoxrThtc2z0kQ1hf/0a/E7qL+HO/7K6Bkav3KfSJOeA3WHw==" }
                });

            migrationBuilder.InsertData(
                table: "UserRoleMap",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773") },
                    { 2, new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773") },
                    { 1, new Guid("53f72fc2-cbda-43fe-90b9-45ed571e4185") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoleMap",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773") });

            migrationBuilder.DeleteData(
                table: "UserRoleMap",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773") });

            migrationBuilder.DeleteData(
                table: "UserRoleMap",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("53f72fc2-cbda-43fe-90b9-45ed571e4185") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("53f72fc2-cbda-43fe-90b9-45ed571e4185"));
        }
    }
}
