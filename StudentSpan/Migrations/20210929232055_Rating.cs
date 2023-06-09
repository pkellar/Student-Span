using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamProject.Migrations
{
    public partial class Rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "StudentListModel",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "StudentListModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "StudentListModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradMonth",
                table: "StudentListModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradYear",
                table: "StudentListModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "StudentListModel");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "StudentListModel");

            migrationBuilder.DropColumn(
                name: "GradMonth",
                table: "StudentListModel");

            migrationBuilder.DropColumn(
                name: "GradYear",
                table: "StudentListModel");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "StudentListModel",
                newName: "Name");
        }
    }
}
