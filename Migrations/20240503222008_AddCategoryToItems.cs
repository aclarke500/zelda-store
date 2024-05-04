using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo_3.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Items",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Items");
        }
    }
}
