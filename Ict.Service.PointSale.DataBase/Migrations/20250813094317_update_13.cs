using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class update_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeWords_PointSales_PointSaleId",
                table: "AlternativeWords");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSales_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Chiefs_PointSales_PointSaleId",
                table: "Chiefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_PointSales_PointSaleId",
                table: "Descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_PointSales_PointSaleId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Logos_PointSales_PointSaleId",
                table: "Logos");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSales_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PointSales_PointSaleId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleActivities_PointSales_PointSaleId",
                table: "PointSaleActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_ClosingStatuses_ClosingStatusId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_CreationTypes_CreationTypeId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OrganizationTypes_OrganizationTypeId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleSchedules_PointSales_PointSaleId",
                table: "PointSaleSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointSales",
                table: "PointSales");

            migrationBuilder.RenameTable(
                name: "PointSales",
                newName: "PointSalesEntities");

            migrationBuilder.RenameIndex(
                name: "IX_PointSales_OwnerTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_OwnerTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSales_OrganizationTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_OrganizationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSales_CreationTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_CreationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSales_ClosingStatusId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_ClosingStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointSalesEntities",
                table: "PointSalesEntities",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeWords_PointSalesEntities_PointSaleId",
                table: "AlternativeWords",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSalesEntities_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity",
                column: "OrganizationsPointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chiefs_PointSalesEntities_PointSaleId",
                table: "Chiefs",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_PointSalesEntities_PointSaleId",
                table: "Descriptions",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_PointSalesEntities_PointSaleId",
                table: "Locations",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logos_PointSalesEntities_PointSaleId",
                table: "Logos",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSalesEntities_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity",
                column: "PointsSalePointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PointSalesEntities_PointSaleId",
                table: "Photos",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleActivities_PointSalesEntities_PointSaleId",
                table: "PointSaleActivities",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleSchedules_PointSalesEntities_PointSaleId",
                table: "PointSaleSchedules",
                column: "PointSaleId",
                principalTable: "PointSalesEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSalesEntities_ClosingStatuses_ClosingStatusId",
                table: "PointSalesEntities",
                column: "ClosingStatusId",
                principalTable: "ClosingStatuses",
                principalColumn: "ClosingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSalesEntities_CreationTypes_CreationTypeId",
                table: "PointSalesEntities",
                column: "CreationTypeId",
                principalTable: "CreationTypes",
                principalColumn: "CreationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSalesEntities_OrganizationTypes_OrganizationTypeId",
                table: "PointSalesEntities",
                column: "OrganizationTypeId",
                principalTable: "OrganizationTypes",
                principalColumn: "OrganizationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSalesEntities_OwnerTypes_OwnerTypeId",
                table: "PointSalesEntities",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeWords_PointSalesEntities_PointSaleId",
                table: "AlternativeWords");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSalesEntities_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Chiefs_PointSalesEntities_PointSaleId",
                table: "Chiefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_PointSalesEntities_PointSaleId",
                table: "Descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_PointSalesEntities_PointSaleId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Logos_PointSalesEntities_PointSaleId",
                table: "Logos");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSalesEntities_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PointSalesEntities_PointSaleId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleActivities_PointSalesEntities_PointSaleId",
                table: "PointSaleActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleSchedules_PointSalesEntities_PointSaleId",
                table: "PointSaleSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSalesEntities_ClosingStatuses_ClosingStatusId",
                table: "PointSalesEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSalesEntities_CreationTypes_CreationTypeId",
                table: "PointSalesEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSalesEntities_OrganizationTypes_OrganizationTypeId",
                table: "PointSalesEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSalesEntities_OwnerTypes_OwnerTypeId",
                table: "PointSalesEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointSalesEntities",
                table: "PointSalesEntities");

            migrationBuilder.RenameTable(
                name: "PointSalesEntities",
                newName: "PointSales");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_OwnerTypeId",
                table: "PointSales",
                newName: "IX_PointSales_OwnerTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_OrganizationTypeId",
                table: "PointSales",
                newName: "IX_PointSales_OrganizationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_CreationTypeId",
                table: "PointSales",
                newName: "IX_PointSales_CreationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_ClosingStatusId",
                table: "PointSales",
                newName: "IX_PointSales_ClosingStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointSales",
                table: "PointSales",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeWords_PointSales_PointSaleId",
                table: "AlternativeWords",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSales_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity",
                column: "OrganizationsPointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chiefs_PointSales_PointSaleId",
                table: "Chiefs",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_PointSales_PointSaleId",
                table: "Descriptions",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_PointSales_PointSaleId",
                table: "Locations",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logos_PointSales_PointSaleId",
                table: "Logos",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSales_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity",
                column: "PointsSalePointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PointSales_PointSaleId",
                table: "Photos",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleActivities_PointSales_PointSaleId",
                table: "PointSaleActivities",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_ClosingStatuses_ClosingStatusId",
                table: "PointSales",
                column: "ClosingStatusId",
                principalTable: "ClosingStatuses",
                principalColumn: "ClosingStatusId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PointSales_OwnerTypes_OwnerTypeId",
                table: "PointSales",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleSchedules_PointSales_PointSaleId",
                table: "PointSaleSchedules",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
