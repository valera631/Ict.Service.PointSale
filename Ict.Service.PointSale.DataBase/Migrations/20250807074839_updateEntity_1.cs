using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updateEntity_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_CreationTypes_CreationTypeId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                table: "PointSales");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationTypeId",
                table: "PointSales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreationTypeId",
                table: "PointSales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreationDate",
                table: "PointSales",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "OpenDate",
                table: "PointSaleActivities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_CreationTypes_CreationTypeId",
                table: "PointSales",
                column: "CreationTypeId",
                principalTable: "CreationTypes",
                principalColumn: "CreationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                table: "PointSales",
                column: "OrganizationTypeId",
                principalTable: "OrganizationTypes",
                principalColumn: "OrganizationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_CreationTypes_CreationTypeId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                table: "PointSales");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationTypeId",
                table: "PointSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreationTypeId",
                table: "PointSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreationDate",
                table: "PointSales",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "OpenDate",
                table: "PointSaleActivities",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_CreationTypes_CreationTypeId",
                table: "PointSales",
                column: "CreationTypeId",
                principalTable: "CreationTypes",
                principalColumn: "CreationTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                table: "PointSales",
                column: "OrganizationTypeId",
                principalTable: "OrganizationTypes",
                principalColumn: "OrganizationTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
