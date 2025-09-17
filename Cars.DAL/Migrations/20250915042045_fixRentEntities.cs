using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cars.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixRentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPayment_AspNetUsers_UserId",
                table: "RentPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_RentPayment_Rents_RentId",
                table: "RentPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentPayment",
                table: "RentPayment");

            migrationBuilder.DropIndex(
                name: "IX_RentPayment_RentId",
                table: "RentPayment");

            migrationBuilder.DropIndex(
                name: "IX_RentPayment_UserId",
                table: "RentPayment");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RentPayment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RentPayment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RentPayment");

            migrationBuilder.AddColumn<string>(
                name: "Drop_Off_location",
                table: "Rents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pick_up_location",
                table: "Rents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "RentPayment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "RentPayment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentPayment",
                table: "RentPayment",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentPayment_AppUserId",
                table: "RentPayment",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPayment_AspNetUsers_AppUserId",
                table: "RentPayment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPayment_Rents_RentId",
                table: "RentPayment",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "RentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentPayment_AspNetUsers_AppUserId",
                table: "RentPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_RentPayment_Rents_RentId",
                table: "RentPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentPayment",
                table: "RentPayment");

            migrationBuilder.DropIndex(
                name: "IX_RentPayment_AppUserId",
                table: "RentPayment");

            migrationBuilder.DropColumn(
                name: "Drop_Off_location",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Pick_up_location",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "RentPayment");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "RentPayment");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Rents",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RentPayment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RentPayment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RentPayment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentPayment",
                table: "RentPayment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RentPayment_RentId",
                table: "RentPayment",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentPayment_UserId",
                table: "RentPayment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPayment_AspNetUsers_UserId",
                table: "RentPayment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentPayment_Rents_RentId",
                table: "RentPayment",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "RentId");
        }
    }
}
