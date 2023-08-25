using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Migrations
{
    /// <inheritdoc />
    public partial class Migration_100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DM80");

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "DM80",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "DM80",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberUserGroups",
                schema: "DM80",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    GroupId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberUserGroups", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_MemberUserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "DM80",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberUserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "DM80",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "DM80",
                table: "Groups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "축구 동아리" },
                    { 2L, "농구 동아리" }
                });

            migrationBuilder.InsertData(
                schema: "DM80",
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1L, "bak@gmail.com", "박영석", "password" },
                    { 2L, "lee@gmail.com", "이건우", "password" },
                    { 3L, "jo@gmail.com", "조범희", "password" },
                    { 4L, "an@gmail.com", "안성윤", "password" },
                    { 5L, "jang@gmail.com", "장동계", "password" }
                });

            migrationBuilder.InsertData(
                schema: "DM80",
                table: "MemberUserGroups",
                columns: new[] { "GroupId", "UserId", "Id" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 1L, 2L },
                    { 1L, 2L, 3L },
                    { 2L, 3L, 4L },
                    { 1L, 4L, 5L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberUserGroups_GroupId",
                schema: "DM80",
                table: "MemberUserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberUserGroups_Id",
                schema: "DM80",
                table: "MemberUserGroups",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberUserGroups",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "DM80");
        }
    }
}
