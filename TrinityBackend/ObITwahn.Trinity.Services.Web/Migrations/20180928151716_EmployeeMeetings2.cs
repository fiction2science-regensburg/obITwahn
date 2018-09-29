using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ObITwahn.Trinity.Services.Web.Migrations
{
    public partial class EmployeeMeetings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emloyees_Meetings_MeetingId",
                table: "Emloyees");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Emloyees_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Emloyees_MeetingId",
                table: "Emloyees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Emloyees");

            migrationBuilder.CreateTable(
                name: "EmployeeMeeting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: true),
                    MeetingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMeeting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeMeeting_Emloyees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Emloyees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeMeeting_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeMeeting_EmployeeId",
                table: "EmployeeMeeting",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeMeeting_MeetingId",
                table: "EmployeeMeeting",
                column: "MeetingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeMeeting");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Meetings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId",
                table: "Emloyees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Emloyees_MeetingId",
                table: "Emloyees",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emloyees_Meetings_MeetingId",
                table: "Emloyees",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Emloyees_EmployeeId",
                table: "Meetings",
                column: "EmployeeId",
                principalTable: "Emloyees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
