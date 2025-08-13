 
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class update_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PointSaleActivities_PointSaleId",
                table: "PointSaleActivities");

            migrationBuilder.DropIndex(
                name: "IX_Logos_OpenDate",
                table: "Logos");

            migrationBuilder.DropIndex(
                name: "IX_Logos_PointSaleId",
                table: "Logos");

            migrationBuilder.DropIndex(
                name: "IX_Locations_OpenDate",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PointSaleId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_OpenDate",
                table: "Descriptions");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_PointSaleId",
                table: "Descriptions");

            migrationBuilder.DropIndex(
                name: "IX_Chiefs_OpenDate",
                table: "Chiefs");

            migrationBuilder.DropIndex(
                name: "IX_Chiefs_PointSaleId",
                table: "Chiefs");

            migrationBuilder.CreateIndex(
                name: "IX_PointSaleActivities_PointSaleId_OpenDate",
                table: "PointSaleActivities",
                columns: new[] { "PointSaleId", "OpenDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Logos_PointSaleId_OpenDate",
                table: "Logos",
                columns: new[] { "PointSaleId", "OpenDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PointSaleId_OpenDate",
                table: "Locations",
                columns: new[] { "PointSaleId", "OpenDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_PointSaleId_OpenDate",
                table: "Descriptions",
                columns: new[] { "PointSaleId", "OpenDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_PointSaleId_OpenDate",
                table: "Chiefs",
                columns: new[] { "PointSaleId", "OpenDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PointSaleActivities_PointSaleId_OpenDate",
                table: "PointSaleActivities");

            migrationBuilder.DropIndex(
                name: "IX_Logos_PointSaleId_OpenDate",
                table: "Logos");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PointSaleId_OpenDate",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_PointSaleId_OpenDate",
                table: "Descriptions");

            migrationBuilder.DropIndex(
                name: "IX_Chiefs_PointSaleId_OpenDate",
                table: "Chiefs");

            migrationBuilder.CreateIndex(
                name: "IX_PointSaleActivities_PointSaleId",
                table: "PointSaleActivities",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Logos_OpenDate",
                table: "Logos",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Logos_PointSaleId",
                table: "Logos",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OpenDate",
                table: "Locations",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PointSaleId",
                table: "Locations",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_OpenDate",
                table: "Descriptions",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_PointSaleId",
                table: "Descriptions",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_OpenDate",
                table: "Chiefs",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_PointSaleId",
                table: "Chiefs",
                column: "PointSaleId");
        }
    }
}
