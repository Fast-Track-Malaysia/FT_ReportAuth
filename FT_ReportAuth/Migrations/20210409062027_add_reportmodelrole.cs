using Microsoft.EntityFrameworkCore.Migrations;

namespace FT_ReportAuth.Migrations
{
    public partial class add_reportmodelrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportModelRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpModelId = table.Column<int>(nullable: true),
                    UserRoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportModelRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportModelRoles_ReportModels_SpModelId",
                        column: x => x.SpModelId,
                        principalTable: "ReportModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportModelRoles_AspNetRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "c6300616-cfe9-4fb9-9ac6-2507eae3fe9f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                column: "ConcurrencyStamp",
                value: "c0ece7cc-4808-48b4-9bbd-a920b6e79214");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                column: "ConcurrencyStamp",
                value: "0d316bfe-90ac-4721-9623-3f7134667a39");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "294a1668-333c-4a3a-9c08-c98a22420a18", "AQAAAAEAACcQAAAAEFF3kc4aI8T2YOtcXLaFulZkRYL5acbtlnMtwcBHfvWE6O33FJAgPTLXtb/S1J64LA==", "38fb5644-7106-4408-bf15-e4bbd44bd905" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportModelRoles_SpModelId",
                table: "ReportModelRoles",
                column: "SpModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportModelRoles_UserRoleId",
                table: "ReportModelRoles",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportModelRoles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "0e58514c-7468-4fa0-8854-282779fea7a5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                column: "ConcurrencyStamp",
                value: "46a20908-2cbe-4dfe-97f7-54c13334a3e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                column: "ConcurrencyStamp",
                value: "f7a56b29-c49f-4322-82d2-9a08a9977894");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71a59fdb-1db5-47b2-814c-c955504f2afb", "AQAAAAEAACcQAAAAEN/otIroIqCb5Y5/hD5OTw+j/NGSuF8m2ZR0tjqX6E2IbGJrF+R5pFKwYBO+IbpoXg==", "b3751bda-c421-4893-b619-3fffaf4c8c1d" });
        }
    }
}
