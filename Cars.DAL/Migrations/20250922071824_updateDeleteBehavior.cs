using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cars.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_AspNetUsers_UserId",
                table: "Accidents");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_AspNetUsers_UserId",
                table: "Accidents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_AspNetUsers_UserId",
                table: "Accidents");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_AspNetUsers_UserId",
                table: "Accidents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
