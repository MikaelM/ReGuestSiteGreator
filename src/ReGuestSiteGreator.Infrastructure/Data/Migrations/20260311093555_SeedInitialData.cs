using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReGuestSiteGreator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "Id", "CreatedAt", "DefaultData", "Meta", "Name", "Script", "Style", "Template", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Header", "", "", "<p>Header</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Block Footer", "", "", "<p>Block Footer</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Block 1", "", "", "<p>Block 1</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Block 2", "", "", "<p>Block 2</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Block 3", "", "", "<p>Block 3</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "{}", "{}", "Block 4", "", "", "<p>Block 4</p>", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Sitemaps",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "PlanId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Basic Sitemap", new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Premium Sitemap", new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin", "$2a$11$VaYCYAUH0DMR3GwWU0D1HOazM8lv/z0XcE/r1ROlP9euDOhqYumvS", 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "IsHomePage", "MetaDescription", "MetaKeywords", "MetaTitle", "Name", "ParentSlug", "PublishedAt", "SitemapId", "Slug", "SortOrder", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", true, "", "", "", "Home", null, null, new Guid("30000000-0000-0000-0000-000000000001"), "home", 1, 0, "Home", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", false, "", "", "", "Contact Us", null, null, new Guid("30000000-0000-0000-0000-000000000001"), "contact-us", 2, 0, "Contact Us", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", true, "", "", "", "Home", null, null, new Guid("30000000-0000-0000-0000-000000000002"), "home", 1, 0, "Home", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", false, "", "", "", "Contact Us", null, null, new Guid("30000000-0000-0000-0000-000000000002"), "contact-us", 2, 0, "Contact Us", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000005"), "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", false, "", "", "", "FAQ", null, null, new Guid("30000000-0000-0000-0000-000000000002"), "faq", 3, 0, "FAQ", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PageBlocks",
                columns: new[] { "Id", "BlockId", "PageId", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001"), 2 },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001"), 3 },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000002"), 1 },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000002"), 2 },
                    { new Guid("50000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002"), 3 },
                    { new Guid("50000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000003"), 1 },
                    { new Guid("50000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000003"), 2 },
                    { new Guid("50000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000003"), 3 },
                    { new Guid("50000000-0000-0000-0000-000000000010"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000003"), 4 },
                    { new Guid("50000000-0000-0000-0000-000000000011"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000004"), 1 },
                    { new Guid("50000000-0000-0000-0000-000000000012"), new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004"), 2 },
                    { new Guid("50000000-0000-0000-0000-000000000013"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004"), 3 },
                    { new Guid("50000000-0000-0000-0000-000000000014"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000005"), 1 },
                    { new Guid("50000000-0000-0000-0000-000000000015"), new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000005"), 2 },
                    { new Guid("50000000-0000-0000-0000-000000000016"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000005"), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "PageBlocks",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Sitemaps",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Sitemaps",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));
        }
    }
}
