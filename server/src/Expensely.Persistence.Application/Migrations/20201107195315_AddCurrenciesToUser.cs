﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expensely.Persistence.Application.Migrations
{
    public partial class AddCurrenciesToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryCurrency",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCurrency",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrency", x => new { x.UserId, x.Currency });
                    table.ForeignKey(
                        name: "FK_UserCurrency_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_UserCurrency_UserId",
                "UserCurrency",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCurrency");

            migrationBuilder.DropColumn(
                name: "PrimaryCurrency",
                table: "User");
        }
    }
}