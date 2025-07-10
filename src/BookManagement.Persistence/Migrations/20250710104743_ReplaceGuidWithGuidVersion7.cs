using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGuidWithGuidVersion7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedOnUtc_Id",
                table: "Users",
                columns: new[] { "CreatedOnUtc", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedOnUtc_Id",
                table: "Users");
        }
    }
}
