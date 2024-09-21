using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterViewTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomViews_Views_ViewId",
                table: "RoomViews");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.RenameColumn(
                name: "ViewId",
                table: "RoomViews",
                newName: "LocationViewId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomViews_ViewId",
                table: "RoomViews",
                newName: "IX_RoomViews_LocationViewId");

            migrationBuilder.CreateTable(
                name: "LocationViews",
                columns: table => new
                {
                    LocationViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationViews", x => x.LocationViewId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomViews_LocationViews_LocationViewId",
                table: "RoomViews",
                column: "LocationViewId",
                principalTable: "LocationViews",
                principalColumn: "LocationViewId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomViews_LocationViews_LocationViewId",
                table: "RoomViews");

            migrationBuilder.DropTable(
                name: "LocationViews");

            migrationBuilder.RenameColumn(
                name: "LocationViewId",
                table: "RoomViews",
                newName: "ViewId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomViews_LocationViewId",
                table: "RoomViews",
                newName: "IX_RoomViews_ViewId");

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    ViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.ViewId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomViews_Views_ViewId",
                table: "RoomViews",
                column: "ViewId",
                principalTable: "Views",
                principalColumn: "ViewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
