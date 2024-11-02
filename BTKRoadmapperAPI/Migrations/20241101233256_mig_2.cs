using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTKRoadmapperAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "UserPreferences");

            migrationBuilder.RenameColumn(
                name: "AvailableHoursPerWeek",
                table: "UserPreferences",
                newName: "AvailableHoursPerDaily");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableHoursPerDaily",
                table: "UserPreferences",
                newName: "AvailableHoursPerWeek");

            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "UserPreferences",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
