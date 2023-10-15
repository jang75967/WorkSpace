using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "DM80",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "DM80",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberUserGroups",
                schema: "DM80",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Attendees",
                schema: "DM80",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendees_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DM80",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendees_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "DM80",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "DM80",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    Payment = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DM80",
                        principalTable: "Activities",
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
                table: "Activities",
                columns: new[] { "Id", "GroupId", "Title" },
                values: new object[,]
                {
                    { 1L, 1L, "체육대회" },
                    { 2L, 2L, "체육대회" }
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

            migrationBuilder.InsertData(
                schema: "DM80",
                table: "Attendees",
                columns: new[] { "Id", "ActivityId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 1L, 2L },
                    { 3L, 2L, 1L },
                    { 4L, 2L, 3L },
                    { 5L, 2L, 4L },
                    { 6L, 2L, 5L }
                });

            migrationBuilder.InsertData(
                schema: "DM80",
                table: "Expenses",
                columns: new[] { "Id", "ActivityId", "Payment" },
                values: new object[,]
                {
                    { 1L, 1L, 10000f },
                    { 2L, 1L, 10000f },
                    { 3L, 2L, 20000f },
                    { 4L, 2L, 20000f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_GroupId",
                schema: "DM80",
                table: "Activities",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_ActivityId",
                schema: "DM80",
                table: "Attendees",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_UserId",
                schema: "DM80",
                table: "Attendees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ActivityId",
                schema: "DM80",
                table: "Expenses",
                column: "ActivityId");

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
                name: "Attendees",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "MemberUserGroups",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "DM80");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "DM80");
        }
    }
}
