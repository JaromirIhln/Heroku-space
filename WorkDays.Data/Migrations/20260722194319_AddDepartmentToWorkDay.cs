using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkDays.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToWorkDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Department",
                table: "WorkDays",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "WorkDays");
        }
    }
}
