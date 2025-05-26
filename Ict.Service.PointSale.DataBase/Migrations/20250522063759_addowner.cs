using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class addowner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OwnerTypes",
                columns: table => new
                {
                    OwnerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerTypes", x => x.OwnerTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointSales_OwnerTypeId",
                table: "PointSales",
                column: "OwnerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales");

            migrationBuilder.DropTable(
                name: "OwnerTypes");

            migrationBuilder.DropIndex(
                name: "IX_PointSales_OwnerTypeId",
                table: "PointSales");
        }
    }
}
