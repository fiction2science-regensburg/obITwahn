using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ObITwahn.Trinity.Services.Web.Migrations
{
    public partial class EmployeeMeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Meetings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Emloyees_EmployeeId",
                table: "Meetings",
                column: "EmployeeId",
                principalTable: "Emloyees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Emloyees_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Meetings");
        }
    }
}
