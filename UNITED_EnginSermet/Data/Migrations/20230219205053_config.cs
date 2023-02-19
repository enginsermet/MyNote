using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNITED_EnginSermet.Data.Migrations
{
    public partial class config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SubNotes_NoteId",
                table: "SubNotes",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubNotes_Notlar_NoteId",
                table: "SubNotes",
                column: "NoteId",
                principalTable: "Notlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubNotes_Notlar_NoteId",
                table: "SubNotes");

            migrationBuilder.DropIndex(
                name: "IX_SubNotes_NoteId",
                table: "SubNotes");
        }
    }
}
