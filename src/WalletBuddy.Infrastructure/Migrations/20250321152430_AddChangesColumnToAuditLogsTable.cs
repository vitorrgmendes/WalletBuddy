using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletBuddy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChangesColumnToAuditLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "userid",
                table: "auditlogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "changes",
                table: "auditlogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changes",
                table: "auditlogs");

            migrationBuilder.AlterColumn<long>(
                name: "userid",
                table: "auditlogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
