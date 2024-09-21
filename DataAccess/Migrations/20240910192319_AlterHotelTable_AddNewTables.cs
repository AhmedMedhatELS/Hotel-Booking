using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterHotelTable_AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelGalleries_MainImageId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_MainImageId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Hotels");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainImage",
                table: "HotelGalleries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainImage",
                table: "HotelGalleries");

            migrationBuilder.AddColumn<int>(
                name: "MainImageId",
                table: "Hotels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_MainImageId",
                table: "Hotels",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelGalleries_MainImageId",
                table: "Hotels",
                column: "MainImageId",
                principalTable: "HotelGalleries",
                principalColumn: "HotelGalleryId");
        }
    }
}
