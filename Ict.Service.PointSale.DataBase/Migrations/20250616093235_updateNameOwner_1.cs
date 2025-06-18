using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updateNameOwner_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "PointSales",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "PointSales");
        }
    }
}
