using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace org.pos.software.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "tbl_employee",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "tbl_employee");
        }
    }
}
