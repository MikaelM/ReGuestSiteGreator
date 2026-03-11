using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGuestSiteGreator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReorderUsernameColumnInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Backup existing data
            migrationBuilder.Sql(@"CREATE TEMP TABLE ""Users_Backup"" AS SELECT * FROM ""Users"";");

            // Drop foreign key constraints
            migrationBuilder.Sql(@"ALTER TABLE ""Partners"" DROP CONSTRAINT IF EXISTS ""FK_Partners_Users_UserId"";");

            // Drop indexes
            migrationBuilder.DropIndex(name: "IX_Users_Email", table: "Users");
            migrationBuilder.DropIndex(name: "IX_Users_Username", table: "Users");

            // Drop table
            migrationBuilder.DropTable(name: "Users");

            // Recreate table with Username column after Id
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            // Restore data
            migrationBuilder.Sql(@"
                INSERT INTO ""Users"" (""Id"", ""Username"", ""Email"", ""PasswordHash"", ""Role"", ""CreatedAt"", ""UpdatedAt"")
                SELECT ""Id"", ""Username"", ""Email"", ""PasswordHash"", ""Role"", ""CreatedAt"", ""UpdatedAt""
                FROM ""Users_Backup"";
            ");

            // Recreate indexes
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            // Recreate foreign key
            migrationBuilder.Sql(@"
                ALTER TABLE ""Partners"" 
                ADD CONSTRAINT ""FK_Partners_Users_UserId"" 
                FOREIGN KEY (""UserId"") 
                REFERENCES ""Users"" (""Id"") 
                ON DELETE CASCADE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Cannot easily reverse column order, but table will still function correctly
        }
    }
}
