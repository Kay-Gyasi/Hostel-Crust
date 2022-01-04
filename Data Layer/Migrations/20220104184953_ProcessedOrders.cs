using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Layer.Migrations
{
    public partial class ProcessedOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "processedOrders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    OrderNum = table.Column<string>(type: "varchar(10)", nullable: false),
                    isFulfilled = table.Column<bool>(type: "bit", nullable: true),
                    isDelivery = table.Column<bool>(type: "bit", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "varchar(200)", nullable: true),
                    DeliveryLocation = table.Column<string>(type: "varchar(50)", nullable: true),
                    DateOrdered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processedOrders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_processedOrders_users_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "users",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processedOrders_CustomerID",
                table: "processedOrders",
                column: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processedOrders");
        }
    }
}
