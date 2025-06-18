using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class nullOwner_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerTypeId",
                table: "PointSales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "PointSales",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerTypeId",
                table: "PointSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "PointSales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
