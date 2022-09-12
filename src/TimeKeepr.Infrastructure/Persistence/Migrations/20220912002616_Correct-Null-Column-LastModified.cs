using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeKeepr.Infrastructure.Persistence.Migrations
{
    public partial class CorrectNullColumnLastModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77b0b64e-1c41-447b-bbff-ce735e972666");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c07cb0dd-fa50-4f1d-94bd-f752472d2792");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "PtoEntries",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Holidays",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b76dd78c-a9ce-4493-a062-f9c5df61da8f", "fb69ee09-cbf2-4615-9840-725b16efe49e", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb6b1b7e-c33e-4796-a541-861dd226e13f", "beacfa13-0e3d-402e-9971-a746804c0b13", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b76dd78c-a9ce-4493-a062-f9c5df61da8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb6b1b7e-c33e-4796-a541-861dd226e13f");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "PtoEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Holidays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "77b0b64e-1c41-447b-bbff-ce735e972666", "5c72ca4d-7aef-44b3-a8d2-4e312a73aab5", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c07cb0dd-fa50-4f1d-94bd-f752472d2792", "7e8cd449-3120-46d1-af78-99c8d5e145e9", "Admin", "ADMIN" });
        }
    }
}
