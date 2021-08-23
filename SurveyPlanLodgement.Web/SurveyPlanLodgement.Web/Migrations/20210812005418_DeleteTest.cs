using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyPlanLodgement.Web.Migrations
{
    public partial class DeleteTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Journals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Test",
                table: "Journals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
