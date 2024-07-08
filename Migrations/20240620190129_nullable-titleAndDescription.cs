using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIdemo.Migrations
{
    public partial class nullabletitleAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChats_AspNetUsers_UserId",
                table: "ApplicationUserChats");

            migrationBuilder.DropIndex(
                name: "IX_DoctorInfos_UserId",
                table: "DoctorInfos");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c375f7d-4972-4deb-a489-e20b43405628", "AQAAAAEAACcQAAAAEJP1ZDX/kmY1e9xFPSQpg60NPcn4Zuo8BSzH1JJAf0N6JxA3QVF4ATU0J4T2O0R+4A==", "7f594ced-cde2-4dac-9620-0fbc98e463d9" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorInfos_UserId",
                table: "DoctorInfos",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChats_AspNetUsers_UserId",
                table: "ApplicationUserChats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChats_AspNetUsers_UserId",
                table: "ApplicationUserChats");

            migrationBuilder.DropIndex(
                name: "IX_DoctorInfos_UserId",
                table: "DoctorInfos");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71dd4f74-f784-41e5-916b-eb97dfb2c6c0", "AQAAAAEAACcQAAAAEGhQVVaKMMOUEBMPghYLSjqbuUR53TeVhFuKcnkSe62vB/7KJc5CB7AEr14DHDvI+Q==", "864801e9-0c7f-4ae4-b5a9-d1f01c877770" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorInfos_UserId",
                table: "DoctorInfos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChats_AspNetUsers_UserId",
                table: "ApplicationUserChats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
