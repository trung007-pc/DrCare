using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.App.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_PhoneNumber",
                table: "Tenants",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_PhoneNumber",
                table: "Tenants");
        }
    }
}
