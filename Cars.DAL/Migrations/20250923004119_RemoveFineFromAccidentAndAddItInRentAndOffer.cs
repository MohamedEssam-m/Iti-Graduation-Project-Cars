using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cars.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFineFromAccidentAndAddItInRentAndOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fine",
                table: "Accidents");

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "Rents",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "Offers",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fine",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Fine",
                table: "Offers");

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "Accidents",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
