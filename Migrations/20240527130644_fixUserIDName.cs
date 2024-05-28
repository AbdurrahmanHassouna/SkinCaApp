using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIdemo.Migrations
{
    public partial class fixUserIDName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_AspNetUsers_UserId",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "UsreId",
                table: "Banners");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Banners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af1f2568-c179-4867-a0dd-0894332a7d10", "AQAAAAEAACcQAAAAEMH1mCZRNuePpQW3HF68Ng2Iset6m2FyxeB7Gtk82C/FO3Tz5pnBuMpZaliKGLHYKQ==", "5b5f204e-22fb-4b80-9f95-07edb5944487" });

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_AspNetUsers_UserId",
                table: "Banners",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_AspNetUsers_UserId",
                table: "Banners");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Banners",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsreId",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cabe2e08-0d39-4e7d-b41d-c285b5aa51ec", "AQAAAAEAACcQAAAAEHhuWVGDXfMZYV8z/gx97amW0ioUjLMU5sqnigIQdOMZcuKIgDtVmYyvrHmLDKHRng==", "5bbf2436-a233-4377-84c2-8a2ae6cbef31" });

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_AspNetUsers_UserId",
                table: "Banners",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
