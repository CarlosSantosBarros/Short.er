using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Short.er.Migrations
{
    public partial class ColumnRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Urls",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Urls",
                newName: "User");
        }
    }
}
