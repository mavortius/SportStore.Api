﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportStore.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    SupplierId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<long>(nullable: true),
                    Stars = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Ratings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                table: "Ratings",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
