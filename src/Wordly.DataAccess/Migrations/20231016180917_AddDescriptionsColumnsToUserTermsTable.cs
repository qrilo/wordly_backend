﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordly.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionsColumnsToUserTermsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserTerms",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserTerms");
        }
    }
}
