using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpdateIssue.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuestFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Rating_Cleanliness = table.Column<float>(type: "real", nullable: false),
                    Rating_Accuracy = table.Column<float>(type: "real", nullable: false),
                    Rating_CheckIn = table.Column<float>(type: "real", nullable: false),
                    Rating_Communication = table.Column<float>(type: "real", nullable: false),
                    Rating_Location = table.Column<float>(type: "real", nullable: false),
                    Rating_Value = table.Column<float>(type: "real", nullable: false),
                    Rating_OverallRating = table.Column<float>(type: "real", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestFeedbacks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestFeedbacks");
        }
    }
}
