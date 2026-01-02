using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ConfigurationStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addprojects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectEnvironmentId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectEnvironmentId",
                table: "UserGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEnvironments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEnvironments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectEnvironments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectEnvironmentId",
                table: "Users",
                column: "ProjectEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ProjectEnvironmentId",
                table: "UserGroups",
                column: "ProjectEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEnvironments_ProjectId",
                table: "ProjectEnvironments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_ProjectEnvironments_ProjectEnvironmentId",
                table: "UserGroups",
                column: "ProjectEnvironmentId",
                principalTable: "ProjectEnvironments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProjectEnvironments_ProjectEnvironmentId",
                table: "Users",
                column: "ProjectEnvironmentId",
                principalTable: "ProjectEnvironments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_ProjectEnvironments_ProjectEnvironmentId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProjectEnvironments_ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ProjectEnvironments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_ProjectEnvironmentId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProjectEnvironmentId",
                table: "UserGroups");
        }
    }
}
