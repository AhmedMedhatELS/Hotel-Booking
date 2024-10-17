using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReviewOnColmunToReservationTaple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReviews_ReservationId",
                table: "UserReviews");

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewOn",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_ReservationId",
                table: "UserReviews",
                column: "ReservationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReviews_ReservationId",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "IsReviewOn",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_ReservationId",
                table: "UserReviews",
                column: "ReservationId");
        }
    }
}
