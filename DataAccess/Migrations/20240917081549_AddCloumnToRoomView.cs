using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCloumnToRoomView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelRequests_States_StateId",
                table: "HotelRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_States_StateId",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "NumberRooms",
                table: "RoomViews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "HotelRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelRequests_States_StateId",
                table: "HotelRequests",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_States_StateId",
                table: "Hotels",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelRequests_States_StateId",
                table: "HotelRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_States_StateId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "NumberRooms",
                table: "RoomViews");

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "Hotels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "HotelRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelRequests_States_StateId",
                table: "HotelRequests",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_States_StateId",
                table: "Hotels",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId");
        }
    }
}
