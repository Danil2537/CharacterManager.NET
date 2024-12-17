using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterManager.Migrations
{
    /// <inheritdoc />
    public partial class DiceAgainMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Dice_StartingGoldDiceId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StartingGoldDiceId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Dice_BelongToClassId",
                table: "Dice",
                column: "BelongToClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StartingGoldDiceId",
                table: "Classes",
                column: "StartingGoldDiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Dice_StartingGoldDiceId",
                table: "Classes",
                column: "StartingGoldDiceId",
                principalTable: "Dice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dice_Classes_BelongToClassId",
                table: "Dice",
                column: "BelongToClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Dice_StartingGoldDiceId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dice_Classes_BelongToClassId",
                table: "Dice");

            migrationBuilder.DropIndex(
                name: "IX_Dice_BelongToClassId",
                table: "Dice");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StartingGoldDiceId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StartingGoldDiceId",
                table: "Classes",
                column: "StartingGoldDiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Dice_StartingGoldDiceId",
                table: "Classes",
                column: "StartingGoldDiceId",
                principalTable: "Dice",
                principalColumn: "Id");
        }
    }
}
