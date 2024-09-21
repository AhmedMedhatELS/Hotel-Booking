using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ADDHotelViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LViews",
                columns: table => new
                {
                    LViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LViews", x => x.LViewId);
                });

            migrationBuilder.CreateTable(
                name: "HotelViews",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    LViewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelViews", x => new { x.HotelId, x.LViewId });
                    table.ForeignKey(
                        name: "FK_HotelViews_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelViews_LViews_LViewId",
                        column: x => x.LViewId,
                        principalTable: "LViews",
                        principalColumn: "LViewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelViews_LViewId",
                table: "HotelViews",
                column: "LViewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelViews");

            migrationBuilder.DropTable(
                name: "LViews");
        }
    }
}
