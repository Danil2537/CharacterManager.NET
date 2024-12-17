using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterManager.Migrations
{
    /// <inheritdoc />
    public partial class DiceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_HitDiceId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_HitDiceId",
                table: "Classes",
                column: "HitDiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_HitDiceId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_HitDiceId",
                table: "Classes",
                column: "HitDiceId",
                unique: true);
        }
    }
}
