using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.WebAPI.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cfc2ca19-cb39-408b-8227-f3875d60f450", "ec6af3f8-151a-481f-9828-acbfc1651ee8", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e1be42a6-c6ec-4a99-bcfa-e8a321bbd2de", "56cbb9fa-ecf7-4e39-a767-a6f79cf0166a", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfc2ca19-cb39-408b-8227-f3875d60f450");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1be42a6-c6ec-4a99-bcfa-e8a321bbd2de");
        }
    }
}
