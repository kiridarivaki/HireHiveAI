using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CandidateOptionalForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_AspNetUsers_UserId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Resume_ResumeId",
                table: "Candidate");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResumeId",
                table: "Candidate",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_AspNetUsers_UserId",
                table: "Candidate",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Resume_ResumeId",
                table: "Candidate",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_AspNetUsers_UserId",
                table: "Candidate");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Resume_ResumeId",
                table: "Candidate");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResumeId",
                table: "Candidate",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_AspNetUsers_UserId",
                table: "Candidate",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Resume_ResumeId",
                table: "Candidate",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
