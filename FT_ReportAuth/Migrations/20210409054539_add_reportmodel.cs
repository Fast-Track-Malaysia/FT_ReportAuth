using Microsoft.EntityFrameworkCore.Migrations;

namespace FT_ReportAuth.Migrations
{
    public partial class add_reportmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportName = table.Column<string>(nullable: true),
                    ReportURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportModels", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportModels");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "058e94e6-477f-4880-b6f9-5e54acde2e02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                column: "ConcurrencyStamp",
                value: "b62e6dfe-14f8-4d19-8ff5-62a7e0a5f2cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                column: "ConcurrencyStamp",
                value: "3e57f705-407f-4f6a-b81c-95fe2d53d444");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a7938ef5-6d83-4677-8b9b-34e5958e03c7", "AQAAAAEAACcQAAAAEAkWUFntZzgAhiWX5EaeSUAbzkG0zlhiRBb3hkGUyomGaMCtIAO3Adj6XX1M+oBIyA==", "058c69ad-3873-4a68-856c-d2b0ee3d521d" });
        }
    }
}
