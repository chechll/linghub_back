using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace linghub.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TEXT",
                columns: table => new
                {
                    id_text = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    text_name = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans1 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans2 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans3 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKText", x => x.id_text);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    surname = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKUser", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                columns: table => new
                {
                    id_word = table.Column<int>(type: "int", nullable: false),
                    enword = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ruword = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ensent = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    rusent = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ans1 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans2 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ans3 = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKword", x => x.id_word);
                });

            migrationBuilder.CreateTable(
                name: "calendar",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    datum = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKkalendar", x => x.id);
                    table.ForeignKey(
                        name: "R_4",
                        column: x => x.id,
                        principalTable: "USER",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "U_text",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    id_text = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKUtext", x => x.id);
                    table.ForeignKey(
                        name: "R_5",
                        column: x => x.id_user,
                        principalTable: "USER",
                        principalColumn: "id_user");
                    table.ForeignKey(
                        name: "R_6",
                        column: x => x.id_text,
                        principalTable: "TEXT",
                        principalColumn: "id_text");
                });

            migrationBuilder.CreateTable(
                name: "U_word",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    id_word = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKUword", x => x.id);
                    table.ForeignKey(
                        name: "R_7",
                        column: x => x.id_user,
                        principalTable: "USER",
                        principalColumn: "id_user");
                    table.ForeignKey(
                        name: "R_8",
                        column: x => x.id_word,
                        principalTable: "Word",
                        principalColumn: "id_word");
                });

            migrationBuilder.CreateIndex(
                name: "IX_U_text_id_text",
                table: "U_text",
                column: "id_text");

            migrationBuilder.CreateIndex(
                name: "IX_U_text_id_user",
                table: "U_text",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_U_word_id_user",
                table: "U_word",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_U_word_id_word",
                table: "U_word",
                column: "id_word");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calendar");

            migrationBuilder.DropTable(
                name: "U_text");

            migrationBuilder.DropTable(
                name: "U_word");

            migrationBuilder.DropTable(
                name: "TEXT");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "Word");
        }
    }
}
