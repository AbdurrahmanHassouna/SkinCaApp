using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIdemo.Migrations
{
    public partial class dataEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Diseases",
                newName: "Symptoms");

            migrationBuilder.AddColumn<string>(
                name: "Causes",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosticMethods",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prevention",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Types",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e9d64b05-e3c9-4d63-84bc-db1356c5ec3b", "AQAAAAEAACcQAAAAENjccfM8OEVyzmvCb4pVlBJ9BV7UCQqBv8sa5jpMMtct5/Egt69inaQuiAP0YnDBCg==", "5277d615-e137-4b47-903a-35e460b119a5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Causes",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "DiagnosticMethods",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Prevention",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Types",
                table: "Diseases");

            migrationBuilder.RenameColumn(
                name: "Symptoms",
                table: "Diseases",
                newName: "Body");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af1f2568-c179-4867-a0dd-0894332a7d10", "AQAAAAEAACcQAAAAEMH1mCZRNuePpQW3HF68Ng2Iset6m2FyxeB7Gtk82C/FO3Tz5pnBuMpZaliKGLHYKQ==", "5b5f204e-22fb-4b80-9f95-07edb5944487" });
        }
    }
}
