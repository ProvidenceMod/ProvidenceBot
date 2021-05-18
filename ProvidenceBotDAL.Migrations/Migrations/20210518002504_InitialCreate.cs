using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProvidenceBotDAL.Migrations.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Suggestion",
          columns: table => new
          {
            ID = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
            Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Author = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Number = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Suggestion", x => x.ID);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Suggestion");
    }
  }
}
