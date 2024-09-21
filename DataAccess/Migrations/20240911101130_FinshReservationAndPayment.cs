using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FinshReservationAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_RoomUnits_RoomUnitId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RoomUnitId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "RoomUnitId",
                table: "Reservations",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "NumberOfGuests",
                table: "Reservations",
                newName: "ReservationStatus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReservationRooms",
                columns: table => new
                {
                    ReservationRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    RoomUnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRooms", x => x.ReservationRoomId);
                    table.ForeignKey(
                        name: "FK_ReservationRooms_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationRooms_RoomUnits_RoomUnitId",
                        column: x => x.RoomUnitId,
                        principalTable: "RoomUnits",
                        principalColumn: "RoomUnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_ReservationId",
                table: "ReservationRooms",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_RoomUnitId",
                table: "ReservationRooms",
                column: "RoomUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationRooms");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Reservations",
                newName: "RoomUnitId");

            migrationBuilder.RenameColumn(
                name: "ReservationStatus",
                table: "Reservations",
                newName: "NumberOfGuests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserReviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomUnitId",
                table: "Reservations",
                column: "RoomUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_RoomUnits_RoomUnitId",
                table: "Reservations",
                column: "RoomUnitId",
                principalTable: "RoomUnits",
                principalColumn: "RoomUnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
