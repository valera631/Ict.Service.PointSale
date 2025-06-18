﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class update_05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnglishNamePointSale",
                table: "PointSaleActivities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishNamePointSale",
                table: "PointSaleActivities");
        }
    }
}
