using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameTracker.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class GameDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");
        }
    }
}
