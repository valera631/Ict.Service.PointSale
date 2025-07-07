using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class newTableAlternativeWord_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlternativeWords",
                columns: table => new
                {
                    AlternativeWordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlternativeWordName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeWords", x => x.AlternativeWordId);
                    table.ForeignKey(
                        name: "FK_AlternativeWords_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPointSales",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPointSales", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PointSaleSchedules",
                columns: table => new
                {
                    PointSaleScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakEndTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSaleSchedules", x => x.PointSaleScheduleId);
                    table.ForeignKey(
                        name: "FK_PointSaleSchedules_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPointSalePointSaleEntity",
                columns: table => new
                {
                    CategoryPointSalesCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationsPointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPointSalePointSaleEntity", x => new { x.CategoryPointSalesCategoryId, x.OrganizationsPointSaleId });
                    table.ForeignKey(
                        name: "FK_CategoryPointSalePointSaleEntity_CategoryPointSales_CategoryPointSalesCategoryId",
                        column: x => x.CategoryPointSalesCategoryId,
                        principalTable: "CategoryPointSales",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPointSalePointSaleEntity_PointSales_OrganizationsPointSaleId",
                        column: x => x.OrganizationsPointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeWords_PointSaleId",
                table: "AlternativeWords",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPointSalePointSaleEntity_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity",
                column: "OrganizationsPointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSaleSchedules_PointSaleId",
                table: "PointSaleSchedules",
                column: "PointSaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternativeWords");

            migrationBuilder.DropTable(
                name: "CategoryPointSalePointSaleEntity");

            migrationBuilder.DropTable(
                name: "PointSaleSchedules");

            migrationBuilder.DropTable(
                name: "CategoryPointSales");
        }
    }
}
