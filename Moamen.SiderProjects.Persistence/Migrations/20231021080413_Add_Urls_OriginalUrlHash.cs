using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moamen.SiderProjects.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Urls_OriginalUrlHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalUrlHash",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalUrlHash",
                table: "Urls");
        }
    }
}
