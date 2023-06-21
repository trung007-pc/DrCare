using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.App.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"),
                column: "ConcurrencyStamp",
                value: "f53fd358-8f1d-42ed-803a-22e263b9bffc");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                columns: new[] { "ConcurrencyStamp", "DeletionTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea92b41e-c2bb-4817-9f38-6f983d1dcc86", new DateTime(2023, 6, 21, 19, 52, 49, 474, DateTimeKind.Local).AddTicks(6508), "AQAAAAEAACcQAAAAEHMyhuB+Ht3t0BC8kMgwkdVFw7uEjJFTryvuFSeWYtlDfdQPM2rZOEPxje7bbhkCHQ==", "cc8ebe9f-dcaa-497d-9969-4ba5c9957443" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "DeletionTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "347e7fee-a988-4ed2-b133-3f61bd447294", new DateTime(2023, 6, 21, 17, 9, 27, 795, DateTimeKind.Local).AddTicks(8605), "AQAAAAEAACcQAAAAENLZ3Q/kfr0Vj3/bqFtbSttoKLGRrBtwsyfYKsiqk2P09qhIjixGIwh0m50PaB/8zg==", null });
        }
    }
}
