using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ict.Service.PointSale.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class update_1301 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "PointSaleEntities");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_OwnerTypeId",
                table: "PointSaleEntities",
                newName: "IX_PointSaleEntities_OwnerTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_OrganizationTypeId",
                table: "PointSaleEntities",
                newName: "IX_PointSaleEntities_OrganizationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_CreationTypeId",
                table: "PointSaleEntities",
                newName: "IX_PointSaleEntities_CreationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSalesEntities_ClosingStatusId",
                table: "PointSaleEntities",
                newName: "IX_PointSaleEntities_ClosingStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointSaleEntities",
                table: "PointSaleEntities",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeWords_PointSaleEntities_PointSaleId",
                table: "AlternativeWords",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSaleEntities_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity",
                column: "OrganizationsPointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chiefs_PointSaleEntities_PointSaleId",
                table: "Chiefs",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_PointSaleEntities_PointSaleId",
                table: "Descriptions",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_PointSaleEntities_PointSaleId",
                table: "Locations",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logos_PointSaleEntities_PointSaleId",
                table: "Logos",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSaleEntities_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity",
                column: "PointsSalePointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PointSaleEntities_PointSaleId",
                table: "Photos",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleActivities_PointSaleEntities_PointSaleId",
                table: "PointSaleActivities",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleEntities_ClosingStatuses_ClosingStatusId",
                table: "PointSaleEntities",
                column: "ClosingStatusId",
                principalTable: "ClosingStatuses",
                principalColumn: "ClosingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleEntities_CreationTypes_CreationTypeId",
                table: "PointSaleEntities",
                column: "CreationTypeId",
                principalTable: "CreationTypes",
                principalColumn: "CreationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleEntities_OrganizationTypes_OrganizationTypeId",
                table: "PointSaleEntities",
                column: "OrganizationTypeId",
                principalTable: "OrganizationTypes",
                principalColumn: "OrganizationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleEntities_OwnerTypes_OwnerTypeId",
                table: "PointSaleEntities",
                column: "OwnerTypeId",
                principalTable: "OwnerTypes",
                principalColumn: "OwnerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointSaleSchedules_PointSaleEntities_PointSaleId",
                table: "PointSaleSchedules",
                column: "PointSaleId",
                principalTable: "PointSaleEntities",
                principalColumn: "PointSaleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeWords_PointSaleEntities_PointSaleId",
                table: "AlternativeWords");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPointSalePointSaleEntity_PointSaleEntities_OrganizationsPointSaleId",
                table: "CategoryPointSalePointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Chiefs_PointSaleEntities_PointSaleId",
                table: "Chiefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_PointSaleEntities_PointSaleId",
                table: "Descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_PointSaleEntities_PointSaleId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Logos_PointSaleEntities_PointSaleId",
                table: "Logos");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorPointSaleEntity_PointSaleEntities_PointsSalePointSaleId",
                table: "OperatorPointSaleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PointSaleEntities_PointSaleId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleActivities_PointSaleEntities_PointSaleId",
                table: "PointSaleActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleEntities_ClosingStatuses_ClosingStatusId",
                table: "PointSaleEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleEntities_CreationTypes_CreationTypeId",
                table: "PointSaleEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleEntities_OrganizationTypes_OrganizationTypeId",
                table: "PointSaleEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleEntities_OwnerTypes_OwnerTypeId",
                table: "PointSaleEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PointSaleSchedules_PointSaleEntities_PointSaleId",
                table: "PointSaleSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointSaleEntities",
                table: "PointSaleEntities");

            migrationBuilder.RenameTable(
                name: "PointSaleEntities",
                newName: "PointSalesEntities");

            migrationBuilder.RenameIndex(
                name: "IX_PointSaleEntities_OwnerTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_OwnerTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSaleEntities_OrganizationTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_OrganizationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSaleEntities_CreationTypeId",
                table: "PointSalesEntities",
                newName: "IX_PointSalesEntities_CreationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PointSaleEntities_ClosingStatusId",
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
    }
}
