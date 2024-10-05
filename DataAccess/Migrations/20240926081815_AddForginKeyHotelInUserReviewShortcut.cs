using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddForginKeyHotelInUserReviewShortcut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "UserReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_HotelId",
                table: "UserReviews",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_Hotels_HotelId",
                table: "UserReviews",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_Hotels_HotelId",
                table: "UserReviews");

            migrationBuilder.DropIndex(
                name: "IX_UserReviews_HotelId",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "UserReviews");
        }
    }
}
