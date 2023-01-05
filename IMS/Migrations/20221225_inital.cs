using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Cryptography.X509Certificates;

#nullable disable

namespace IMF.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            this._createAccountTable(migrationBuilder);
            this._createPositionTable(migrationBuilder);
            this._createSymbolTable(migrationBuilder);
        }
        private void _createAccountTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: true)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cash = table.Column<string>(type: "FLOAT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                });
        }
        private void _createPositionTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: true)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sod = table.Column<string>(type: "FLOAT", nullable: false),
                    Executed = table.Column<string>(type: "FLOAT", nullable: false),
                    Reserved = table.Column<string>(type: "FLOAT", nullable: false),
                    SymoblId = table.Column<string>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<string>(type: "INTEGER", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                });
        }
        private void _createSymbolTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: true)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sedol = table.Column<string>(type: "CHAR", nullable: false),
                    InstrumentType = table.Column<string>(type: "CHAR", nullable: false),
                    Exchange = table.Column<string>(type: "CHAR", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
            migrationBuilder.DropTable(
                name: "Positions");
            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
