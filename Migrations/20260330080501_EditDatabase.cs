using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class EditDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipPhone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipPhone",
                table: "Orders");
        }
    }
}
