using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabAws.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIdQueueLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QueueId",
                table: "upload_logs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueueId",
                table: "upload_logs");
        }
    }
}
