using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cars.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingsDictionaryInCarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Cars");
        }
    }
}
