using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyPlanLodgement.Web.Migrations
{
    public partial class LodgementModelModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandType",
                table: "Lodgements");

            migrationBuilder.AddColumn<int>(
                name: "VerificationOfficerId",
                table: "Lodgements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationOfficerId",
                table: "Lodgements");

            migrationBuilder.AddColumn<string>(
                name: "LandType",
                table: "Lodgements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
