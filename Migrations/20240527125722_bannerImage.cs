using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIdemo.Migrations
{
    public partial class bannerImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Banners",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cabe2e08-0d39-4e7d-b41d-c285b5aa51ec", "AQAAAAEAACcQAAAAEHhuWVGDXfMZYV8z/gx97amW0ioUjLMU5sqnigIQdOMZcuKIgDtVmYyvrHmLDKHRng==", "5bbf2436-a233-4377-84c2-8a2ae6cbef31" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Banners");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9ff81c2-6dd1-4ee0-a4cf-fb3d4a5186ce", "AQAAAAEAACcQAAAAEIobKF2h/qjLj7zwlNGLjyWWr71708espR5CQ/n0nLon8Wc+vX/KmSZr+MBHuRhU3Q==", "0126bbba-c3bd-4a2b-b9ab-25ad43b5bd52" });
        }
    }
}
