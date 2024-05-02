using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo_3.Migrations.UserDbMigrations
{
    /// <inheritdoc />
    public partial class AddBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balence",
                table: "Users",
                newName: "Balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Users",
                newName: "Balence");
        }
    }
}
