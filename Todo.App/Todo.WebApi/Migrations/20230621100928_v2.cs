using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.App.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "Todo.Users", new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") },
                    { 2, "permission", "Todo.Users.Authorize", new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") },
                    { 3, "permission", "Todo.Roles", new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") },
                    { 4, "permission", "Todo.Roles.Authorize", new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"),
                column: "ConcurrencyStamp",
                value: "f9cb2a20-f440-451a-ab32-f52f58a669f1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                columns: new[] { "ConcurrencyStamp", "DeletionTime", "IsActive", "PasswordHash" },
                values: new object[] { "347e7fee-a988-4ed2-b133-3f61bd447294", new DateTime(2023, 6, 21, 17, 9, 27, 795, DateTimeKind.Local).AddTicks(8605), true, "AQAAAAEAACcQAAAAENLZ3Q/kfr0Vj3/bqFtbSttoKLGRrBtwsyfYKsiqk2P09qhIjixGIwh0m50PaB/8zg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"),
                column: "ConcurrencyStamp",
                value: "a448ca19-7bd0-42ad-84b4-d6d6ab8c3e6c");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                columns: new[] { "ConcurrencyStamp", "DeletionTime", "IsActive", "PasswordHash" },
                values: new object[] { "00661cb6-659d-4c25-891b-ca16d24431b7", new DateTime(2023, 6, 21, 16, 50, 24, 995, DateTimeKind.Local).AddTicks(4021), false, "AQAAAAEAACcQAAAAEE8dyDHcygoqdwxyVscY65f471Qb473MVqfpAHlf602ci4c9+EH1ldjI+d7jVF2fVg==" });
        }
    }
}
