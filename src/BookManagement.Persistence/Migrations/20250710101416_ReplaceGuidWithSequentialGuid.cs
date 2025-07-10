using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGuidWithSequentialGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_CreatedOnUtc_Id",
                table: "Books",
                columns: new[] { "CreatedOnUtc", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_CreatedOnUtc_Id",
                table: "Books");
        }
    }
}
