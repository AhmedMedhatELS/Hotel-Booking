using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCounteryColumnToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CountryId",
                table: "Reservations",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Countries_CountryId",
                table: "Reservations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Countries_CountryId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CountryId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Reservations");
        }
    }
}
