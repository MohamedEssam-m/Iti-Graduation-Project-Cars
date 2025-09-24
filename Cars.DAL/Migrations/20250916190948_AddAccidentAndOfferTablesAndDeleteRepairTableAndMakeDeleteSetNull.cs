#nullable disable

namespace Cars.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAccidentAndOfferTablesAndDeleteRepairTableAndMakeDeleteSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairPayment_Repairs_RepairId",
                table: "RepairPayment");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_RepairPayment_RepairId",
                table: "RepairPayment");

            migrationBuilder.RenameColumn(
                name: "RepairId",
                table: "RepairPayment",
                newName: "accidentId");

            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    AccidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accidents", x => x.AccidentId);
                    table.ForeignKey(
                        name: "FK_Accidents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accidents_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "CarId");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentId = table.Column<int>(type: "int", nullable: false),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_Offers_Accidents_AccidentId",
                        column: x => x.AccidentId,
                        principalTable: "Accidents",
                        principalColumn: "AccidentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_AspNetUsers_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairPayment_accidentId",
                table: "RepairPayment",
                column: "accidentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_carId",
                table: "Accidents",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_UserId",
                table: "Accidents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_AccidentId",
                table: "Offers",
                column: "AccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_MechanicId",
                table: "Offers",
                column: "MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairPayment_Accidents_accidentId",
                table: "RepairPayment",
                column: "accidentId",
                principalTable: "Accidents",
                principalColumn: "AccidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairPayment_Accidents_accidentId",
                table: "RepairPayment");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_RepairPayment_accidentId",
                table: "RepairPayment");

            migrationBuilder.RenameColumn(
                name: "accidentId",
                table: "RepairPayment",
                newName: "RepairId");

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MechanicResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProposedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repairs_AspNetUsers_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repairs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Repairs_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairPayment_RepairId",
                table: "RepairPayment",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_CarId",
                table: "Repairs",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_MechanicId",
                table: "Repairs",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_UserId",
                table: "Repairs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairPayment_Repairs_RepairId",
                table: "RepairPayment",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id");
        }
    }
}
