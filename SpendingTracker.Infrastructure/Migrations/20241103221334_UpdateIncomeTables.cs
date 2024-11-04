using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIncomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userIncome");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "income",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "income",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "categoryIncome",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_income_AccountId",
                table: "income",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_categoryIncome_UserId",
                table: "categoryIncome",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_categoryIncome_users_UserId",
                table: "categoryIncome",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_income_accounts_AccountId",
                table: "income",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categoryIncome_users_UserId",
                table: "categoryIncome");

            migrationBuilder.DropForeignKey(
                name: "FK_income_accounts_AccountId",
                table: "income");

            migrationBuilder.DropIndex(
                name: "IX_income_AccountId",
                table: "income");

            migrationBuilder.DropIndex(
                name: "IX_categoryIncome_UserId",
                table: "categoryIncome");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "income");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "income");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "categoryIncome");

            migrationBuilder.CreateTable(
                name: "userIncome",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userIncome", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userIncome_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userIncome_income_IncomeId",
                        column: x => x.IncomeId,
                        principalTable: "income",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userIncome_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userIncome_AccountId",
                table: "userIncome",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_userIncome_IncomeId",
                table: "userIncome",
                column: "IncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_userIncome_UserId",
                table: "userIncome",
                column: "UserId");
        }
    }
}
