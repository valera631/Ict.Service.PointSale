using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class Сreatedatabase_21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiefPositions",
                columns: table => new
                {
                    ChiefPositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiefPositions", x => x.ChiefPositionId);
                });

            migrationBuilder.CreateTable(
                name: "ClosingStatuses",
                columns: table => new
                {
                    ClosingStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosingStatuses", x => x.ClosingStatusId);
                });

            migrationBuilder.CreateTable(
                name: "CreationTypes",
                columns: table => new
                {
                    CreationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreationTypes", x => x.CreationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.OperatorId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTypes",
                columns: table => new
                {
                    OrganizationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTypes", x => x.OrganizationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PointSales",
                columns: table => new
                {
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTypeId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerTypeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationTypeId = table.Column<int>(type: "int", nullable: false),
                    ClosingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosingStatusId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsAproved = table.Column<bool>(type: "bit", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSales", x => x.PointSaleId);
                    table.ForeignKey(
                        name: "FK_PointSales_ClosingStatuses_ClosingStatusId",
                        column: x => x.ClosingStatusId,
                        principalTable: "ClosingStatuses",
                        principalColumn: "ClosingStatusId");
                    table.ForeignKey(
                        name: "FK_PointSales_CreationTypes_CreationTypeId",
                        column: x => x.CreationTypeId,
                        principalTable: "CreationTypes",
                        principalColumn: "CreationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                        column: x => x.OrganizationTypeId,
                        principalTable: "OrganizationTypes",
                        principalColumn: "OrganizationTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chiefs",
                columns: table => new
                {
                    ChiefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ChiefName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefPositionId = table.Column<int>(type: "int", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAproved = table.Column<bool>(type: "bit", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chiefs", x => x.ChiefId);
                    table.ForeignKey(
                        name: "FK_Chiefs_ChiefPositions_ChiefPositionId",
                        column: x => x.ChiefPositionId,
                        principalTable: "ChiefPositions",
                        principalColumn: "ChiefPositionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chiefs_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Descriptions",
                columns: table => new
                {
                    DescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DescriptionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descriptions", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_Descriptions_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAproved = table.Column<bool>(type: "bit", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logos",
                columns: table => new
                {
                    LogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.LogoId);
                    table.ForeignKey(
                        name: "FK_Logos_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorPointSaleEntity",
                columns: table => new
                {
                    OperatorsOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsSalePointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorPointSaleEntity", x => new { x.OperatorsOperatorId, x.PointsSalePointSaleId });
                    table.ForeignKey(
                        name: "FK_OperatorPointSaleEntity_Operators_OperatorsOperatorId",
                        column: x => x.OperatorsOperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperatorPointSaleEntity_PointSales_PointsSalePointSaleId",
                        column: x => x.PointsSalePointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointSaleActivities",
                columns: table => new
                {
                    PointSaleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamePointSale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSaleActivities", x => x.PointSaleActivityId);
                    table.ForeignKey(
                        name: "FK_PointSaleActivities_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "PointSaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_ChiefPositionId",
                table: "Chiefs",
                column: "ChiefPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_OpenDate",
                table: "Chiefs",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_PointSaleId",
                table: "Chiefs",
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
                name: "IX_Locations_OpenDate",
                table: "Locations",
                column: "OpenDate");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PointSaleId",
                table: "Locations",
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
                name: "IX_OperatorPointSaleEntity_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity",
                column: "PointsSalePointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PointSaleId",
                table: "Photos",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSaleActivities_PointSaleId",
                table: "PointSaleActivities",
                column: "PointSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSales_ClosingStatusId",
                table: "PointSales",
                column: "ClosingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSales_CreationTypeId",
                table: "PointSales",
                column: "CreationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSales_OrganizationTypeId",
                table: "PointSales",
                column: "OrganizationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chiefs");

            migrationBuilder.DropTable(
                name: "Descriptions");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Logos");

            migrationBuilder.DropTable(
                name: "OperatorPointSaleEntity");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PointSaleActivities");

            migrationBuilder.DropTable(
                name: "ChiefPositions");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "PointSales");

            migrationBuilder.DropTable(
                name: "ClosingStatuses");

            migrationBuilder.DropTable(
                name: "CreationTypes");

            migrationBuilder.DropTable(
                name: "OrganizationTypes");
        }
    }
}
