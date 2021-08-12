using Microsoft.EntityFrameworkCore.Migrations;

namespace ticketBotApi.Migrations
{
    public partial class addeddiscordRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "discord", "00000000-0000-0000-0000-000000000000", "Discord", "DISCORD" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "discord");
        }
    }
}
